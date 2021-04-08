using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using Collada141;

namespace DestinyColladaGenerator
{
	public class canvasContainer
	{
		public string plateId { get; set; }
		public string textureId { get; set; }
		public Bitmap canvas { get; set; }

		public canvasContainer(string plate, string texture, Bitmap canv)
		{
			plateId = plate;
			textureId = texture;
			canvas = canv;
		}
	}

	class WriteCollada
	{
		//Alt checkRenderPart, using Spasm's method.
		public static bool checkRenderPart(dynamic staticPart) {
			bool shouldRender = Program.disableLodCulling;

			dynamic part = staticPart;
			
			string lodCategoryName = part["lodCategory"].GetProperty("name").GetString();
			
			if (lodCategoryName.IndexOf('0') >= 0) shouldRender = true;
			if (lodCategoryName.IndexOf("unused") >= 0) {shouldRender = true; Console.WriteLine("Found LOD category \"unused\".");}
			if (lodCategoryName.IndexOf("count") >= 0) {shouldRender = true; Console.WriteLine("Found LOD category \"count\".");}
			
			return shouldRender;
		}

		public static string multiOutItemName = "";
		
		public static void WriteFile(List<dynamic> renderModels, string writeLocation, string game)
		{
			CultureInfo ci = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			List<string> bytecodes = new List<string>();
			
			// D2 uses some different values than D1. Game-dependent values will be assigned here.
			int defaultShader = 9;

			if (game == "2")
			{
				defaultShader = 7;
			}
			
			string folderName = "DestinyModel";
			if (Program.multipleFolderOutput) folderName = multiOutItemName+"_";
			int fileNum = 0;
			while( Directory.Exists(Path.Combine(writeLocation, $"{folderName}{fileNum.ToString()}")) ) 
			{
				fileNum++;
			}
			string OutLoc = Path.Combine(writeLocation, $"{folderName}{fileNum.ToString()}");
			Directory.CreateDirectory(OutLoc);
			
			COLLADA model = COLLADA.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "template.dae"));

			DateTime rightNow = DateTime.UtcNow;

			model.asset.created = rightNow;
			model.asset.modified = rightNow;
			model.asset.unit.meter = 1D;
			model.asset.unit.name = "meter";

			library_geometries libGeoms = model.Items[1] as library_geometries;
			List<geometry> geoms = new List<geometry>();
			geometry geomTemplate = libGeoms.geometry[0];
			
			library_controllers libControls = model.Items[2] as library_controllers;
			List<controller> controls = new List<controller>();
			//controller controlTemplate = libControls.controller[0];
			
			library_visual_scenes libScenes = model.Items[3] as library_visual_scenes;
			List<node> sceneNodes = new List<node>();
			node nodeTemplate = libScenes.visual_scene[0].node[0];
			node riggedNodeTemplate = libScenes.visual_scene[0].node[1];

			List<string> skeletonsInScene = new List<string>();

			int vertexOffset = 0;
			
			int riggedMeshes = 0;
			
			foreach (dynamic renderModel in renderModels)
			{
				List<dynamic> renderMeshes = renderModel.meshes;
				dynamic renderTextures = renderModel.textures;
				string modelName = renderModel.name;
				string modelType = renderModel.type;
				List<dynamic> renderRaws = renderModel.raws;
				modelName = Regex.Replace(modelName, @"[^A-Za-z0-9\.]", "-");

				string templateType = "armor";
				int boneCount = 72;
				//if (modelType == "Ghost Shell") {templateType = "ghost"; boneCount = 13;}
				//else if (modelType == "Vehicle") {templateType = "sparrow"; boneCount = 10;}
				switch(modelType)
				{
					case ("Ghost Shell"):
						templateType = "ghost";
						boneCount = 13;
						break;
					case ("Vehicle"):
						templateType = "sparrow";
						boneCount = 10;
						break;
					case ("Hand Cannon"):
						templateType = "handcannon";
						boneCount = 8;
						break;
					case ("Combat Bow"):
						templateType = "bow";
						boneCount = 25;
						break;
					case ("Machine Gun"):
						templateType = "lmg";
						boneCount = 15;
						break;
					default:
						templateType = "armor";
						boneCount = 72;
						break;
				}

				COLLADA skeletonSource = COLLADA.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{templateType}.dae"));

				int skeletonIndex = 0;
				if (skeletonsInScene.Contains(templateType)) skeletonIndex = skeletonsInScene.IndexOf(templateType);
				else 
				{
					skeletonsInScene.Add(templateType);
					skeletonIndex = skeletonsInScene.Count-1;

					library_visual_scenes skeletonLibScenes = skeletonSource.Items[3] as library_visual_scenes;
					node skeleton = skeletonLibScenes.visual_scene[0].node[0];
					skeleton.id = skeleton.id.Replace("NUMBER", $"{skeletonIndex}");
					node pedestal = skeleton.node1[0] as node;
					pedestal.id = pedestal.id.Replace("NUMBER", $"{skeletonIndex}");
					skeleton.node1[0] = pedestal;
					sceneNodes.Add(skeleton);
				}

				library_controllers skeletonLibControls = skeletonSource.Items[2] as library_controllers;
				controller controlTemplate = skeletonLibControls.controller[0];

				// Geometry
				for (var m=0; m<renderMeshes.Count; m++) 
				//Parallel.For(0, renderMeshes.Count, m =>
				{
					dynamic renderMesh = renderMeshes[m];
					dynamic indexBuffer = renderMesh.indexBuffer;
					dynamic vertexBuffer = renderMesh.vertexBuffer;
					dynamic skinBuffer = renderMesh.skinBuffer;
					//dynamic dataDrivenVertexBuffer = renderMesh.dataDrivenVertexBuffer;
					dynamic positionOffset = renderMesh.positionOffset;
					dynamic positionScale = renderMesh.positionScale;
					dynamic texcoord0ScaleOffset = renderMesh.texcoord0ScaleOffset;
					dynamic texcoordOffset = renderMesh.texcoordOffset;
					dynamic texcoordScale = renderMesh.texcoordScale;
					dynamic parts = renderMesh.parts;

					if (parts.Count == 0) {
						//ConsoleEx.Warn("Skipped RenderMesh["+geometryHash+":"+m+"]: No parts");
						//return; 
						continue;
					} // Skip meshes with no parts


					// Spasm.Renderable.prototype.render
					var partCount = -1;
					//foreach (var part in parts)
					for (int pIndex=0; pIndex<parts.Count; pIndex++)
					{
						var part = parts[pIndex];
						if (!checkRenderPart(part)) continue;

						geometry geom = geomTemplate.Copy<geometry>();

						mesh meshObj = geom.Item as mesh;
						
						bool doRigging = false;

						List<source> semanticSources = new List<source>();
						List<StringBuilder> semanticValues = new List<StringBuilder>();
						List<string> semanticNames = new List<string>();
						List<int> semanticCounts = new List<int>();

						meshObj.vertices.id = "vertices0";
						meshObj.vertices.input[0].source = "#position0";

						StringBuilder parray = new StringBuilder();

						partCount++;

						string mN = $"{m.ToString("00")}.{partCount.ToString("000")}";

						geom.id = modelName+"_"+mN+"-mesh";
						geom.name = modelName+"."+mN;

						int gearDyeSlot = part["gearDyeSlot"];
						int transparencyType = 0;

						int flags = part["flags"].GetInt32();
						int shader = defaultShader;
						int variant = (int)part["variantShaderIndex"].GetInt32();
						if (part.ContainsKey("shader")) shader = part["shader"].type.GetInt32();
						//else if (part.variantShaderIndex != -1) shader = -1;

						//if (shader != defaultShader) transparencyType = 24; // If a piece uses a nonstandard shader, mark it.
						//if (shader == -1) transparencyType = 32; // If a piece uses "no shader"(?), use a different marking.
						
						if (game == "") // Check for known D1 shaders
						{
							switch (shader)
							{
								case -1: // Unknown. Sight?
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses unknown-shader-1.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 1: // Unknown emissive.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses unknown-emissive-1.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 7: // Decal.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses decal.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 8: // Controlled emissive.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses controlled emissive.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 9: // Default.
									if (variant==-1) {}
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 11: // Emissive decal.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses emissive decal.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								default:
									Console.WriteLine($"Unknown shader {shader} in {modelName} mesh {m} part {partCount}.");
									break;
							}
						}
						else // Check for known D2 shaders
						{
							switch (shader)
							{
								case 2: // No backface culling.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses no backface culling.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 7: // Default. Variants: 
									if (variant==-1) {}
									else if (variant!=2) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses default with transmission.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								case 8: // UV alpha fade.
									if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses UV alpha fade.");
									else Console.WriteLine($"Unknown variant {variant} in {modelName} mesh {m} part {partCount}.");
									break;
								default:
									Console.WriteLine($"Unknown shader {shader} in {modelName} mesh {m} part {partCount}.");
									break;
							}
						}
						if ((flags & 0x8) != 0) {Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses alpha clip."); transparencyType = 8;} // Mark alpha test use.

						if (game=="" || part["primitiveType"].GetInt32() == 3)
						{
							// Load Vertex Stream
							int increment = 3;
							int start = (int)part["indexStart"].GetInt32();
							int count = (int)part["indexCount"].GetInt32();

							// PrimitiveType, 3=TRIANGLES, 5=TRIANGLE_STRIP
							// https://stackoverflow.com/questions/3485034/convert-triangle-strips-to-triangles

							int firstValue = 0;
							if (part["primitiveType"].GetInt32() == 5) {
								increment = -1;
								count -= 2;
								firstValue = count-1;
							}

							for (int i=firstValue; i<count&&0<=i; i+= increment) 
							{
								int faceIndex = start+i;

								int[] tri = ((int)part["primitiveType"].GetInt32()) == 3 || ((i & 1) == 0) ? new int[3]{0, 1, 2} : new int[3]{2, 1, 0};

								if (indexBuffer[faceIndex+0] == 65535 || indexBuffer[faceIndex+1] == 65535 || indexBuffer[faceIndex+2] == 65535) continue;

								for (var j=0; j<3; j++) 
								{
									int index = (int) indexBuffer[faceIndex+tri[j]];
									dynamic vertex = vertexBuffer[index];

									if (vertex == null) { // Verona Mesh
										Console.WriteLine("MissingVertex["+index+"]");
										i=count;
										break;
									}

									gearDyeSlot = part["gearDyeSlot"];
									//if (game == "2")
									//	gearDyeSlot = vertexBuffer[index]["normal0_raw"][6] & 0x7;
									//Console.WriteLine(vertexBuffer[index]["normal0_raw"][6] & 0x7);
									
									if(vertexBuffer[index].ContainsKey("slots") && vertexBuffer[index]["slots"] != gearDyeSlot)
									{
										vertexBuffer.Add((Dictionary<string,dynamic>) ObjectExtensions.Copy(vertexBuffer[index]));
										indexBuffer[faceIndex+tri[j]] = (ushort) (vertexBuffer.Count-1);
										index = vertexBuffer.Count-1;
									}

									parray.Append(index);
									parray.Append(' ');
									parray.Append(gearDyeSlot+transparencyType);
									parray.Append(' ');

									vertexBuffer[index]["slots"] = gearDyeSlot;
									if(!vertexBuffer[index].ContainsKey("uv1"))
										vertexBuffer[index].Add("uv1", new double[]{5.0,5.0});
								}
							}
						}
						else if (game=="2")
						{
							List<List<int>> strips = new List<List<int>>();
							strips.Add(new List<int>());

							for (int p=0; p<part["indexCount"].GetInt32(); p++)
							{
								int index = indexBuffer[part["indexStart"].GetInt32()+p];
								if (index == 0xFFFF)
								{
									strips.Add(new List<int>());
									continue;
								}

								//if(vertexBuffer[index].ContainsKey("slots") && vertexBuffer[index]["slots"] != gearDyeSlot)
								//{
								//	vertexBuffer.Add((Dictionary<string,dynamic>) ObjectExtensions.Copy(vertexBuffer[index]));
								//	indexBuffer[part["indexStart"].GetInt32()+p] = (ushort) (vertexBuffer.Count-1);
								//	index = vertexBuffer.Count-1;
								//}

								strips[strips.Count - 1].Add(index);

								//if ((vertexBuffer[index]["normal0"]&0x80)==0x80)
								gearDyeSlot = vertexBuffer[index]["normal0_raw"][6] & 0x7;
								//else
								vertexBuffer[index]["slots"] = gearDyeSlot;
								if(!vertexBuffer[index].ContainsKey("uv1"))
									vertexBuffer[index].Add("uv1", new double[]{5.0,5.0});
							}

							//foreach (List<int> strip in strips)
							//{
							//	for (int v=0; v <  strip.Count - 2; v++)
							//	{
							//		if ((v&1) == 1)
							//			foreach (int vp in new int[]{0,1,2})
							//			{
							//				parray.Append(strip[v+vp]+" ");
							//				parray.Append(vertexBuffer[strip[v+vp]]["slots"]+transparencyType+" ");
							//			} 
							//		else
							//		foreach (int vp in new int[]{0,2,1})
							//			{
							//				parray.Append(strip[v+vp]+" ");
							//				parray.Append(vertexBuffer[strip[v+vp]]["slots"]+transparencyType+" ");
							//			} 
							//	}
							//}
							foreach (List<int> strip in strips)
							{
								//	for (int v=0; v <  strip.Count - 2; v++)
								for (int v=(strip.Count - 3); 0 <= v; v--)
								{
									if ((v&1) == 1)
										foreach (int vp in new int[]{2,1,0}) // {0,1,2}
										{
											parray.Append(strip[v+vp]+" ");
											parray.Append(vertexBuffer[strip[v+vp]]["slots"]+transparencyType+" ");
										} 
									else
									foreach (int vp in new int[]{1,2,0}) // {0,2,1}
										{
											parray.Append(strip[v+vp]+" ");
											parray.Append(vertexBuffer[strip[v+vp]]["slots"]+transparencyType+" ");
										} 
								}
							}
						}

					controller control = controlTemplate.Copy<controller>();
					control.id = modelName+"_"+mN+"-skin";
					control.name = modelName+"_Skin."+mN;
					skin skinItem = control.Item as skin;
					//skinItem.bind_shape_matrix = $"1 0 0 {positionOffset[1].GetDouble()} 0 1 0 {positionOffset[0].GetDouble()*-1} 0 0 1 {positionOffset[2].GetDouble()} 0 0 0 1";
					skinItem.source1 = "#"+modelName+"_"+mN+"-mesh";	
					skinItem.joints.input[0].source = "#"+modelName+"-"+mN+"-skin-joints";
					skinItem.joints.input[1].source = "#"+modelName+"-"+mN+"-skin-bind_poses";
					skinItem.vertex_weights.count = (ulong) vertexBuffer.Count;
					skinItem.vertex_weights.input[0].source = "#"+modelName+"-"+mN+"-skin-joints";
					skinItem.vertex_weights.input[1].source = "#"+modelName+"-"+mN+"-skin-weights";
					StringBuilder vcountArray = new StringBuilder();
					StringBuilder varray = new StringBuilder();
					
					skinItem.source[0].id = modelName+"-"+mN+"-skin-joints";
					skinItem.source[0].technique_common.accessor.source = "#"+modelName+"-"+mN+"-skin-joints-array";
					Name_array jointNames = skinItem.source[0].Item as Name_array;
					jointNames.id = modelName+"-"+mN+"-skin-joints-array";
					skinItem.source[0].Item = jointNames;
					
					skinItem.source[1].id = modelName+"-"+mN+"-skin-bind_poses";
					skinItem.source[1].technique_common.accessor.source = "#"+modelName+"-"+mN+"-skin-bind_poses-array";
					float_array bindPoses = skinItem.source[1].Item as float_array;
					bindPoses.id = modelName+"-"+mN+"-skin-bind_poses-array";
					skinItem.source[1].Item = bindPoses;
					
					skinItem.source[2].id = modelName+"-"+mN+"-skin-weights";
					skinItem.source[2].technique_common.accessor.source = "#"+modelName+"-"+mN+"-skin-weights-array";
					skinItem.source[2].technique_common.accessor.count = (ulong) vertexBuffer.Count * 4;
					skinItem.source[2].technique_common.accessor.stride = 1;
					float_array skinWeights = skinItem.source[2].Item as float_array;
					skinWeights.id = modelName+"-"+mN+"-skin-weights-array";
					skinWeights.count = (ulong) vertexBuffer.Count * 4;
					List<double> weightsList = new List<double>();

					int weightCount = 0;

					if (!vertexBuffer[0].ContainsKey("slots")) vertexBuffer[0].Add("slots",0);
					if (!vertexBuffer[0].ContainsKey("uv1")) vertexBuffer[0].Add("uv1",new double[]{5.0,5.0});
					
					foreach (dynamic vSemantic in vertexBuffer[0]) // Generate vertex data layout
					{
						string semName = vSemantic.Key;
						source meshSource = new source();
						meshSource.id = semName;
						meshSource.name = semName;

						float_array valueArray = new float_array();
						valueArray.id = $"{semName}-array";
						meshSource.Item = valueArray;

						sourceTechnique_common techniqueCommon = new sourceTechnique_common();
						accessor techniqueAccessor = new accessor();
						techniqueAccessor.source = $"#{semName}-array";
						
						bool skipSemantic = false;
						switch (Regex.Replace(semName, @"[0-9]", ""))
						{
							case "position":
							case "normal":
							case "tangent":
								param pX = new param(); pX.name="X"; pX.type="float";
								param pY = new param(); pY.name="Y"; pY.type="float";
								param pZ = new param(); pZ.name="Z"; pZ.type="float";
								techniqueAccessor.param = new param[]{pX, pY, pZ};
								techniqueAccessor.stride = 3;
								break;
							case "uv":
							//case "shader":
								param pS = new param(); pS.name="S"; pS.type="float";
								param pT = new param(); pT.name="T"; pT.type="float";
								techniqueAccessor.param = new param[]{pS, pT};
								techniqueAccessor.stride = 2;
								break;
							case "color":
							case "slots":
								param pR = new param(); pR.name="R"; pR.type="float";
								param pG = new param(); pG.name="G"; pG.type="float";
								param pB = new param(); pB.name="B"; pB.type="float";
								param pA = new param(); pA.name="A"; pA.type="float";
								techniqueAccessor.param = new param[]{pR, pG, pB, pA};
								techniqueAccessor.stride = 4;
								break;
							default: skipSemantic = true; break;
						}
						if (skipSemantic) continue;

						if (semName == "slots")
						{
							valueArray.count = 128;
							valueArray._Text_ = "0.333 0 0    1   0.666 0 0    1   0.999 0 0 1      0 0.333 0    1   0 0.666 0    1   0 0.999 0    1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 0.25 1   0.666 0 0.25 1   0.999 0 0.25 1   0 0.333 0.25 1   0 0.666 0.25 1   0 0.999 0.25 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 0.5  1   0.666 0 0.5  1   0.999 0 0.5 1    0 0.333 0.5  1   0 0.666 0.5  1   0 0.999 0.5  1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 0.75 1   0.666 0 0.75 1   0.999 0 0.75 1   0 0.333 0.75 1   0 0.666 0.75 1   0 0.999 0.75 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 1    1   0.666 0 1    1   0.999 0 1 1      0 0.333 1    1   0 0.666 1    1   0 0.999 1    1   0.750 0.750 0.750 1   0.750 0.750 0.750 1";
							techniqueAccessor.count = 32;
							meshSource.name = "slots";
						}

						techniqueCommon.accessor = techniqueAccessor;
						meshSource.Item = valueArray;
						meshSource.technique_common = techniqueCommon;

						semanticSources.Add(meshSource);
						semanticValues.Add(new StringBuilder());
						semanticNames.Add(semName);
						semanticCounts.Add(0);
					}
					
					int lastBlendValue = 0;
					int lastBlendCount = 0;
					for (var v=0; v<vertexBuffer.Count; v++) 
					{
						dynamic vertex = vertexBuffer[v];
						var position = vertex["position0"];
						if(!vertex.ContainsKey("uv1"))
						{
							vertex.Add("uv1", new double[]{5.0, 5.0});
						}

						foreach (dynamic vElement in vertex)
						{
							string eName = vElement.Key;
							int index = semanticNames.IndexOf(eName);
							if (eName == "slots" || eName == "blendweight0" || eName == "blendindices0" || eName.Contains("_raw")) continue;
							if (index == -1) {Console.WriteLine($"Vertex {v} has an element ({eName}) not found in vertex 0."); continue;}

							var eValues = vElement.Value;
							
							switch(Regex.Replace(eName, @"[0-9]", ""))
							{
								case "position":
									//float tempVal = (float) eValues[0];
									semanticValues[index].Append($"{eValues[1]} {eValues[0] * -1} {eValues[2]} ");
									break;
								case "normal":
								case "tangent":
									semanticValues[index].Append($"{eValues[1]} {eValues[0]*-1} {eValues[2]} ");
									break;
								case "uv":
									float texcoordX = (float)(((double)vertex["uv0"][0])*texcoordScale[0].GetDouble()+texcoordOffset[0].GetDouble());
									float texcoordY = (float)(((double)vertex["uv0"][1])*texcoordScale[1].GetDouble()+texcoordOffset[1].GetDouble());
									if (eName != "uv0")
									{
										texcoordX *= (float) eValues[0];
										texcoordY *= (float) eValues[1];
									}
									semanticValues[index].Append($"{texcoordX} {1-texcoordY} ");
									break;
								case "color":
									semanticValues[index].Append($"{eValues[0]} {eValues[1]} {eValues[2]} {eValues[3]*0} ");
									break;
								default: break;
							}

							semanticCounts[index]++;
						}

						if ((vertex.ContainsKey("blendindices0")) || (vertex["position0"][3] != 255)) doRigging = true;
						
						if (doRigging)
						{
							// Set bone weights
							float boneIndex = (float)position[3];//Math.abs((positionOffset[3] * 32767.0) + 0.01);

							//function parserSkinBuffer
							//(
							//	skinBuffer: SkinEntryData,
								byte[] blendValue = BitConverter.GetBytes(boneIndex);
							//	vertexBuffer: VertexData
							//) 
							if (skinBuffer != null && game=="2" && !vertex.ContainsKey("blendindices0"))
							{
								double[] indices = new double[]{0, 0, 0, 0};
								float[] weights = new float[]{1, 0, 0, 0};

								int blendIndex = (int) boneIndex;
								int blendFlags = BitConverter.ToInt32(blendValue) & 0xf800;

								int totalBones = 0;
								int bufferSize = 0;

								//if (blendFlags == 0) {
								if (0 <= boneIndex && boneIndex <= 255) {
									indices[0] = blendIndex;
									totalBones = 1;
								//} else if (blendFlags == 0x800) {
								} else if (boneIndex >= 2048) {
									blendIndex = blendIndex-2048;
									bufferSize = 2;
								//} else if (blendFlags == 0xf000) {
								} else if (boneIndex < -2048) {
									blendIndex = Math.Abs(blendIndex)-2048;// & 0x7ff;
									bufferSize = 4;
								} else {
									ConsoleEx.Warn($"TGXParser:Skin {boneIndex} {blendFlags}");
								}

								SkinBufferChunk blendData = null;
								int blendCount = 0;

								if (bufferSize > 0) {
									if (lastBlendValue != blendIndex) {
										lastBlendCount = 0;
									}
									lastBlendValue = blendIndex;

									blendData = skinBuffer.data[blendIndex * 8 + lastBlendCount];
									while (blendData.count == 0) {
										lastBlendCount++;
										blendData = skinBuffer.data[blendIndex * 8 + lastBlendCount];
									}
									totalBones = blendData.count;
									for (int i=0; i<blendData.count; i++) {
										indices[i] = blendData.indices[i];
										weights[i] = (float)(blendData.weights[i]/255.0);
									}
									blendCount = totalBones > 2 ? 2 : 1;
								}

								lastBlendCount += blendCount;

								//vertexBuffer.blendindices = indices;
								//vertexBuffer.blendweight = weights.Select<float,double>((w) => ((float)w) / 255.0);
								vertex["blendindices0"] = indices;
								vertex["blendweight0"] = weights;
							}

							double[] blendIndices = !vertex.ContainsKey("blendindices0") ? new double[]{(double)boneIndex, 255, 255, 255} : new double[] {(double)vertex["blendindices0"][0],(double)vertex["blendindices0"][1],(double)vertex["blendindices0"][2],(double)vertex["blendindices0"][3]};
							double[] blendWeights = !vertex.ContainsKey("blendweight0") ? new double[]{1, 0, 0, 0} : new double[] {(double)vertex["blendweight0"][0],(double)vertex["blendweight0"][1],(double)vertex["blendweight0"][2],(double)vertex["blendweight0"][3]};

							int vertIndices = 1;

							var totalWeights = 0.0;
							for (var w=0; w<blendIndices.Length; w++) {
								var blendIndex = blendIndices[w];
								if (blendIndex == 255) continue;
								if (blendWeights[w] == 0) continue;
								//if (blendIndex%1 != 0) blendIndex = Math.Floor(blendIndex);
								if (blendIndex%1 != 0) 
								{
									byte[] positionWBytes = BitConverter.GetBytes((float)blendIndex);
									string bytestring = $"{positionWBytes[0]:X} {positionWBytes[1]:X} {positionWBytes[2]:X} {positionWBytes[3]:X}"; 
									if (!bytecodes.Contains(bytestring))
									{
										ConsoleEx.Warn(bytestring);
										bytecodes.Add(bytestring);
										blendIndex = bytecodes.Count;
									}
									else
										blendIndex = bytecodes.IndexOf(bytestring);
								}
								blendIndex = blendIndex % boneCount;
								varray.Append(blendIndex+" ");
								varray.Append((weightCount)+" ");
								weightsList.Add((double)blendWeights[w]);
								totalWeights += blendWeights[w];
								weightCount += 1;
								vertIndices += 1;
							}

							if (vertex.ContainsKey("slots")) {varray.Append((vertex["slots"] + boneCount)+" ");}
							else {varray.Append((boneCount+1)+" ");}
							varray.Append((/*weightCount*/ 0)+" ");
							
							vcountArray.Append(vertIndices+" ");
						}
					}
					skinWeights.Values = weightsList.ToArray();
					skinItem.source[2].Item = skinWeights;
					skinItem.vertex_weights.vcount = vcountArray.ToString();
					skinItem.vertex_weights.v = varray.ToString();
					control.Item = skinItem;

					node sceneNode;
					if (doRigging)
					{
						sceneNode = riggedNodeTemplate.Copy<node>();
						sceneNode.instance_controller[0].url = "#"+modelName+"_"+mN+"-skin";
						sceneNode.instance_controller[0].name = modelName+"_Skin."+mN;
						sceneNode.instance_controller[0].skeleton[0] = $"#Armature_{skeletonIndex}_Pedestal";
					}
					else
					{
						sceneNode = nodeTemplate.Copy<node>();
						sceneNode.instance_geometry[0].url = "#"+modelName+"_"+mN+"-mesh";
						sceneNode.instance_geometry[0].name = modelName+"."+mN;
						((matrix)sceneNode.Items[0]).Values = new double[] {1,0,0,0,//positionOffset[1].GetDouble(),
																			0,1,0,0,//positionOffset[0].GetDouble()*-1,
																			0,0,1,0,//positionOffset[2].GetDouble(),
																			0,0,0,1};
					}
					sceneNode.id = modelName+"_"+mN;
					sceneNode.name = modelName+"."+mN;

					if(doRigging)
					{
						controls.Add(control);
						riggedMeshes += 1;
					}
					sceneNodes.Add(sceneNode);

					vertexOffset += vertexBuffer.Count;

					List<InputLocalOffset> triInputs = new List<InputLocalOffset>();
					for (int e=0; e<semanticNames.Count; e++)
					{
						string eName = semanticNames[e];
						if (eName != "slots")
						{
							semanticSources[e].technique_common.accessor.count = (ulong)semanticCounts[e];
							float_array eValues = semanticSources[e].Item as float_array;
							eValues._Text_ = semanticValues[e].ToString();
							
							switch (Regex.Replace(eName, @"[0-9]", ""))
							{
								case "position":
								case "normal":
								case "tangent":
									eValues.count = (ulong)semanticCounts[e] * 3;
									break;
								case "uv":
								//case "shader":
									eValues.count = (ulong)semanticCounts[e] * 2;
									break;
								case "color":
									eValues.count = (ulong)semanticCounts[e] * 4;
									break;
								default: break;
							}

							semanticSources[e].Item = eValues;
						}

						InputLocalOffset triInput = new InputLocalOffset();
						triInput.source = $"#{eName}";
						triInput.offset = 0;
						int inSet = (eName == "slots") ? 0 : Int32.Parse(Regex.Replace(eName, @"[a-zA-Z]", ""));
						triInput.set = (ulong)inSet;
						switch(Regex.Replace(eName, @"[0-9]", ""))
						{
							case "position":
								triInput.semantic = "VERTEX";
								triInput.source = "#vertices0";
								break;
							case "normal":
								triInput.semantic = "NORMAL";
								break;
							case "tangent":
								triInput.semantic = "TANGENT";
								break;
							case "uv":
							//case "shader":
								triInput.semantic = "TEXCOORD";
								break;
							case "color":
								triInput.semantic = "COLOR";
								triInput.set += 1;
								break;
							case "slots":
								triInput.semantic = "COLOR";
								triInput.offset = 1;
								break;
							default: break;
						}
						triInputs.Add(triInput);
						}

					meshObj.source = semanticSources.ToArray();
					
					triangles meshTris = meshObj.Items[0] as triangles;
					meshTris.input = triInputs.ToArray();
					meshTris.p = parray.ToString();
					meshObj.Items[0] = meshTris;

					geom.Item = meshObj;
					geoms.Add(geom);
					}
				}
				//);

				// Textures
				if (renderTextures != null)
				{
					Dictionary<string, canvasContainer> canvasPlates = new Dictionary<string, canvasContainer>();
					Bitmap ctx;
					dynamic plateMetas = renderTextures["texturePlates"];
					//foreach (JArray texturePlates in plateMetas)
					for (int plateMetaIndex=0; plateMetaIndex<plateMetas.Count; plateMetaIndex++)
					{
						dynamic texturePlates = plateMetas[plateMetaIndex];
						if (texturePlates.GetArrayLength() == 1) {
							dynamic texturePlate = texturePlates[0];
							dynamic texturePlateSet = texturePlate.GetProperty("plate_set");

							// Stitch together plate sets
							// Web versions are pre-stitched

							//foreach (var texturePlateProp in texturePlateSet.Properties()) 
							JsonElement.ObjectEnumerator plateProps = texturePlateSet.EnumerateObject();
							while (plateProps.MoveNext())
							{	
								var texturePlateProp = plateProps.Current;
								texturePlate = texturePlateProp.Value;
								string texturePlateId = texturePlateProp.Name;
								string texturePlateRef = texturePlateId+"_"+texturePlate.GetProperty("plate_index");

								string textureId = texturePlateId;
								switch(texturePlateId) {
									case ("diffuse"): textureId = "map"; break;
									case ("normal"): textureId = "normalMap"; break;
									case ("gearstack"): textureId = "gearstackMap"; break;
									case ("dyeslot"): textureId = "dyemap"; break;
									default:
										Console.WriteLine("UnknownTexturePlateId: "+texturePlateId);
										break;
								}

								int canvasWidth = texturePlate.GetProperty("plate_size")[0].GetInt32();
								int canvasHeight = texturePlate.GetProperty("plate_size")[1].GetInt32();
								if (textureId == "dyemap")
								{
									canvasWidth/=4;
									canvasHeight/=4;
								}

								canvasContainer canvasPlate;
								if (!canvasPlates.ContainsKey(texturePlateRef)) {
									ctx = new Bitmap(canvasWidth, canvasHeight);

									canvasPlate = new canvasContainer(
										texturePlateId,
										textureId,
										ctx
									);
									canvasPlates.Add(texturePlateRef, canvasPlate);
								}
								canvasPlate = canvasPlates[texturePlateRef];
								ctx = canvasPlate.canvas;
								
								for (int p=0; p<texturePlate.GetProperty("texture_placements").GetArrayLength(); p++) {
									dynamic placement = texturePlate.GetProperty("texture_placements")[p];
									if (!renderTextures.ContainsKey(placement.GetProperty("texture_tag_name").GetString()))
									{
										Console.WriteLine("Missing plate texture detected. Skipping."); continue;
									}
									byte[] placementTexture = renderTextures[placement.GetProperty("texture_tag_name").GetString()];
									Bitmap imageTex = new Bitmap(new MemoryStream(placementTexture));
									
									if (placementTexture == null) {
										Console.WriteLine("TextureNotLoaded"+placement.texture_tag_name);
										continue;
									}

									// System.Drawing.Graphics zeroes colors with alpha of 0, so I need to manually copy pixels over.
									int placementPosX = placement.GetProperty("position_x").GetInt32();
									int placementPosY = placement.GetProperty("position_y").GetInt32();
									int placementSizeX = placement.GetProperty("texture_size_x").GetInt32();
									int placementSizeY = placement.GetProperty("texture_size_y").GetInt32();
									for (int posX=0; posX<placementSizeX; posX++)
									{
										for (int posY=0; posY<placementSizeY; posY++)
										{
											ctx.SetPixel(posX+placementPosX, posY+placementPosY, imageTex.GetPixel(posX, posY));
										}
									}

									// Write gbits to textures with offsets, mainly for alt sight textures.
									Bitmap gbit = new Bitmap(canvasWidth, canvasHeight);
									for (int posX=0; posX<placementSizeX; posX++)
									{
										for (int posY=0; posY<placementSizeY; posY++)
										{
											gbit.SetPixel(posX+placementPosX, posY+placementPosY, imageTex.GetPixel(posX, posY));
										}
									}
									string ext = "";
									ImageFormat format = ImageFormat.Jpeg;
									if (placementTexture[1] == 'P' && placementTexture[2] == 'N' && placementTexture[3] == 'G') {ext = "png"; format = ImageFormat.Png;}
									else ext = "jpg";

									string directory = Path.Combine(OutLoc, "Textures", modelName.Replace("Female-", ""));
									if (File.Exists(Path.Combine(directory, $"{placement.GetProperty("texture_tag_name").GetString()}.{ext}"))) continue;
									if (!directory.Contains("Male-"))
									{
										if (!Directory.Exists(directory)) 
										{
											Directory.CreateDirectory(directory);
										}
										gbit.Save(Path.Combine(directory, $"{placement.GetProperty("texture_tag_name").GetString()}.{ext}"), format);
									}
								}

								// save the data to a stream
								ctx.Save(Path.Combine(OutLoc, $"{modelName}_{texturePlateRef}.png"), ImageFormat.Png);
							}
						}
						else if (texturePlates.GetArrayLength() > 1) {
							Console.WriteLine("MultipleTexturePlates?");
						}
					}
					GC.KeepAlive(canvasPlates);
					
					
					foreach (string textureName in renderTextures.Keys)
					{
						if (textureName=="texturePlates" || modelName.StartsWith("Male-")) continue;
						byte[] textureFile = renderTextures[textureName];
						string ext = "";
						if (textureFile[1] == 'P' && textureFile[2] == 'N' && textureFile[3] == 'G') ext = "png";
						else if (textureFile[0] == 'D' && textureFile[1] == 'D' && textureFile[2] == 'S') ext = "dds";
						else ext = "jpg";

						string directory = Path.Combine(OutLoc, "Textures", modelName.Replace("Female-", ""));
						if (File.Exists(Path.Combine(directory, $"{textureName}.{ext}"))) continue;
						if (!Directory.Exists(directory)) 
						{
							Directory.CreateDirectory(directory);
						}
						using (FileStream texWriter = new FileStream(Path.Combine(directory, $"{textureName}.{ext}"), FileMode.Create, FileAccess.Write))
						{
							texWriter.Write(textureFile);
						}
					}
				}

				if (renderRaws != null)
				{
					//foreach (dynamic tgxBin in renderRaws)
					for (int r=0; r<renderRaws.Count; r++)
					{
						dynamic tgxBin = renderRaws[r];
						foreach (KeyValuePair<string,dynamic> file in tgxBin.files)
						{
							string directory = Path.Combine(OutLoc, "Raws", modelName);
							if (!Directory.Exists(directory)) 
							{
								Directory.CreateDirectory(directory);
							}
							if (file.Key == "render_metadata.js")
								continue;
							else if (file.Key == "render_metadata_js")
							{
								using (FileStream TGXWriter = new FileStream(Path.Combine(directory, r+"-render_metadata.js"), FileMode.Create, FileAccess.Write))
								{
									TGXWriter.Write(file.Value);
								}
							}
							else
							{
								using (FileStream TGXWriter = new FileStream(Path.Combine(directory, r+"-"+file.Key), FileMode.Create, FileAccess.Write))
								{
									TGXWriter.Write(file.Value.data);
								}
							}
						}
					}
				}
			}
			
			libGeoms.geometry = geoms.ToArray();
			model.Items[1] = libGeoms;
			libControls.controller = controls.ToArray();
			model.Items[2] = libControls;
			libScenes.visual_scene[0].node = sceneNodes.ToArray();
			model.Items[3] = libScenes;

			model.Save(Path.Combine(OutLoc, "model.dae"));

			if (!Directory.Exists(Path.Combine(OutLoc, "Shaders"))) 
			{
				Directory.CreateDirectory(Path.Combine(OutLoc, "Shaders"));
			}

			// Save shader presets
			foreach (KeyValuePair<string, string> kvp in ShaderPresets.presets)
			{
				using (StreamWriter texWriter = new StreamWriter(Path.Combine(OutLoc, "Shaders", $"{Regex.Replace(kvp.Key, @"[^A-Za-z0-9\.]", "-")}.txt")))
				{
					texWriter.Write(kvp.Value);
				}
			}

			Directory.CreateDirectory(Path.Combine(OutLoc, "Shaders", "Blender"));
			Directory.CreateDirectory(Path.Combine(OutLoc, "Shaders", "Unity"));
			//Directory.CreateDirectory(Path.Combine(OutLoc, "Shaders", "UE4"));
			
			// Save nodegen scripts
			foreach (KeyValuePair<string, string> kvp in ShaderPresets.scripts)
			{
				string engine = "";
				string extension = "";
				if (kvp.Key.Contains("_BLENDER"))
				{
					engine = "Blender";
					extension = "py";
				}
				else if (kvp.Key.Contains("_UNITY"))
				{
					engine = "Unity";
					extension = "shader";
				}
				//else if (kvp.Key.Contains("_UNREAL"))
				//{
				//	engine = "UE4";
				//	extension = "ue4.py"
				//}
				using (StreamWriter shaderWriter = new StreamWriter(Path.Combine(OutLoc, "Shaders", engine, $"{Regex.Replace(Regex.Replace(kvp.Key, @"[^A-Za-z0-9\.]", "-"), @"(-UNITY)|(-BLENDER)|(-UNREAL)", "")}.{extension}")))
				{
					string shaderName = Regex.Replace(kvp.Key, @"[^A-Za-z0-9\.]", "-").Replace("-armor","").Replace("-weapon","").Replace("-ghost","").Replace("-sparrow","").Replace("-ship","");
					shaderName = Regex.Replace(shaderName, @"(-UNITY)|(-BLENDER)|(-UNREAL)", "");
					string filledShader = kvp.Value.Replace("OUTPUTPATH", Path.Combine("Textures", shaderName)).Replace("\\", "/");
					shaderWriter.Write(filledShader);
				}
			}
		}
	}
}