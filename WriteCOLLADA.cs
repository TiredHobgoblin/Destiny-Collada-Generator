using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using SkiaSharp;
using Collada141;

namespace DestinyColladaGenerator
{
	public class canvasContainer
	{
	private string plateIdField;
	private string textureIdField;
	private SKSurface canvasField;

	public string plateId
	{
		get { return plateIdField; }
		set { plateIdField = value; }
	}
	public string textureId
	{
		get { return textureIdField; }
		set { textureIdField = value; }
	}
	public SKSurface canvas
	{
		get { return canvasField; }
		set { canvasField = value; }
	}

	public canvasContainer(string plate, string texture, SKSurface canv)
	{
		plateIdField = plate;
		textureIdField = texture;
		canvasField = canv;
	}
	}

	public class canvasArray
	{
	private List<canvasContainer> canvasList = new List<canvasContainer>();
	private List<string> nameList = new List<string>();

	public void AddItem(string name, canvasContainer item)
	{
		canvasList.Add(item);
		nameList.Add(name);
	}

	public canvasContainer get(string name)
	{
		int index = nameList.IndexOf(name);
		return (index != -1) ? canvasList[index] : null;
	}
	}

	class WriteCollada
	{
		//Alt checkRenderPart, using Spasm's method.
		public static bool checkRenderPart(dynamic staticPart) {
			bool shouldRender = false;

			dynamic part = staticPart;
			
			string lodCategoryName = part["lodCategory"].GetProperty("name").GetString();
			
			if (lodCategoryName.IndexOf('0') >= 0) shouldRender = true;
			if (lodCategoryName.IndexOf("unused") >= 0) {shouldRender = true; Console.WriteLine("Found LOD category \"unused\".");}
			if (lodCategoryName.IndexOf("count") >= 0) {shouldRender = true; Console.WriteLine("Found LOD category \"count\".");}
			
			return shouldRender;
		}
		
		public static void WriteFile(List<dynamic> renderModels, string writeLocation, string game)
		{
			CultureInfo ci = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
			
			// D2 uses some different values than D1. Game-dependent values will be assigned here.
			int defaultShader = 9;

			if (game == "2")
			{
				defaultShader = 7;
			}
			
			int fileNum = 0;
			while( Directory.Exists(Path.Combine(new string[]{writeLocation, "DestinyModel"+fileNum.ToString()})) ) 
			{
				fileNum++;
			}
			string OutLoc = Path.Combine(writeLocation, "DestinyModel"+fileNum.ToString());
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
			controller controlTemplate = libControls.controller[0];
			
			library_visual_scenes libScenes = model.Items[3] as library_visual_scenes;
			List<node> sceneNodes = new List<node>();
			node nodeTemplate = libScenes.visual_scene[0].node[0];
			node riggedNodeTemplate = libScenes.visual_scene[0].node[2];

			int vertexOffset = 0;
			
			int riggedMeshes = 0;
			
			foreach (dynamic renderModel in renderModels)
			{
				List<dynamic> renderMeshes = renderModel.meshes;
				dynamic renderTextures = renderModel.textures;
				string modelName = renderModel.name;
				List<dynamic> renderRaws = renderModel.raws;
				modelName = Regex.Replace(modelName, @"[^A-Za-z0-9\.]", "-");

				// Geometry
				for (var m=0; m<renderMeshes.Count; m++) 
				//Parallel.For(0, renderMeshes.Count, m =>
				{
					string mN = $"{m.ToString("00")}.000";

					geometry geom = geomTemplate.Copy<geometry>();

					geom.id = modelName+"_"+mN+"-mesh";
					geom.name = modelName+"."+mN;

					mesh meshObj = geom.Item as mesh;
					
					bool doRigging = false;

					List<source> semanticSources = new List<source>();
					List<StringBuilder> semanticValues = new List<StringBuilder>();
					List<string> semanticNames = new List<string>();
					List<int> semanticCounts = new List<int>();

					meshObj.vertices.id = "vertices0";
					meshObj.vertices.input[0].source = "#position0";

					StringBuilder parray = new StringBuilder();

					dynamic renderMesh = renderMeshes[m];
					dynamic indexBuffer = renderMesh.indexBuffer;
					dynamic vertexBuffer = renderMesh.vertexBuffer;
					dynamic positionOffset = renderMesh.positionOffset;
					dynamic positionScale = renderMesh.positionScale;
					dynamic texcoord0ScaleOffset = renderMesh.texcoord0ScaleOffset;
					dynamic texcoordOffset = renderMesh.texcoordOffset;
					dynamic texcoordScale = renderMesh.texcoordScale;
					dynamic parts = renderMesh.parts;


					if (parts.Count == 0) {
						//Console.WriteLine("Skipped RenderMesh["+geometryHash+":"+m+"]: No parts");
						//return; 
						continue;
					} // Skip meshes with no parts


					// Spasm.Renderable.prototype.render
					var partCount = -1;
					foreach (var part in parts)
					{
						if (!checkRenderPart(part)) continue;

						partCount++;

						int gearDyeSlot = part["gearDyeSlot"];
						int transparencyType = 0;

						int flags = part["flags"].GetInt32();
						int shader = defaultShader;
						int variant = (int)part["variantShaderIndex"].GetInt32();
						if (part.ContainsKey("shader")) shader = part["shader"].type.GetInt32();
						//else if (part.variantShaderIndex != -1) shader = -1;

						if (shader != defaultShader) transparencyType = 24; // If a piece uses a nonstandard shader, mark it.
						if (shader == -1) transparencyType = 32; // If a piece uses "no shader"(?), use a different marking.
						
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

							if (part["primitiveType"].GetInt32() == 5) {
								increment = 1;
								count -= 2;
							}

							for (int i=0; i<count; i+= increment) 
							{
								int faceIndex = start+i;

								int[] tri = ((int)part["primitiveType"].GetInt32()) == 3 || ((i & 1) != 0) ? new int[3]{0, 1, 2} : new int[3]{2, 1, 0};

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

								vertexBuffer[index]["slots"] = gearDyeSlot;
								if(!vertexBuffer[index].ContainsKey("uv1"))
									vertexBuffer[index].Add("uv1", new double[]{5.0,5.0});
							}

							foreach (List<int> strip in strips)
							{
								for (int v=0; v <  strip.Count - 2; v++)
								{
									if ((v&1) == 1)
										foreach (int vp in new int[]{0,1,2})
										{
											parray.Append(strip[v+vp]+" ");
											parray.Append(gearDyeSlot+transparencyType+" ");
										} 
									else
									foreach (int vp in new int[]{0,2,1})
										{
											parray.Append(strip[v+vp]+" ");
											parray.Append(gearDyeSlot+transparencyType+" ");
										} 
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
					//if (vertexBuffer[0].shader0 == null){
					//	vertexBuffer[0].shader0 = new JArray();
					//	vertexBuffer[0].shader0.Add(0);
					//	vertexBuffer[0].shader0.Add(0);
					//}

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
							if (eName == "slots" || eName == "blendweight0" || eName == "blendindices0") continue;
							if (index == -1) {Console.WriteLine($"Vertex {v} has an element ({eName}) not found in vertex 0."); continue;}

							var eValues = vElement.Value;
							
							switch(Regex.Replace(eName, @"[0-9]", ""))
							{
								case "position":
									float tempVal = (float) eValues[0];
									semanticValues[index].Append($"{eValues[1]} {tempVal * -1} {eValues[2]} ");
									break;
								case "normal":
								case "tangent":
									semanticValues[index].Append($"{eValues[0]} {eValues[1]} {eValues[2]} ");
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
									semanticValues[index].Append($"{eValues[0]} {eValues[1]} {eValues[2]} {eValues[3]} ");
									break;
								default: break;
							}

							semanticCounts[index]++;
						}

						if ((vertex.ContainsKey("blendindices0")) || (vertex["position0"][3] != 255)) doRigging = true;
						
						if (doRigging)
						{
							// Set bone weights
							var boneIndex = position[3];//Math.abs((positionOffset[3] * 32767.0) + 0.01);

							double[] blendIndices = !vertex.ContainsKey("blendindices0") ? new double[]{(double)boneIndex, 255, 255, 255} : new double[] {(double)vertex["blendindices0"][0],(double)vertex["blendindices0"][1],(double)vertex["blendindices0"][2],(double)vertex["blendindices0"][3]};
							double[] blendWeights = !vertex.ContainsKey("blendweight0") ? new double[]{1, 0, 0, 0} : new double[] {(double)vertex["blendweight0"][0],(double)vertex["blendweight0"][1],(double)vertex["blendweight0"][2],(double)vertex["blendweight0"][3]};

							int vertIndices = 1;

							var totalWeights = 0.0;
							for (var w=0; w<blendIndices.Length; w++) {
								var blendIndex = blendIndices[w];
								//Console.WriteLine(blendIndex);
								if (blendIndex == 255) break;
								if (blendIndex%1 != 0) blendIndex = Math.Floor(blendIndex);
								blendIndex = blendIndex % 72;
								varray.Append(blendIndex+" ");
								varray.Append((weightCount)+" ");
								weightsList.Add((double)blendWeights[w]);
								totalWeights += blendWeights[w]*255.0;
								weightCount += 1;
								vertIndices += 1;
							}

							if (vertex.ContainsKey("slots")) {varray.Append((vertex["slots"] + 72)+" ");}
							else {varray.Append((73)+" ");}
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
						sceneNode.instance_controller[0].skeleton[0] = "#Armature_Pedestal";
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
				//);

				// Textures
				if (renderTextures != null)
				{
					canvasArray canvasPlates = new canvasArray();
					SKSurface canvas;
					SKCanvas ctx;
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

								int scale = 1;

								//if (texturePlate.texture_placements.Count == 0) {
								//	continue;
								//}

								int canvasWidth = texturePlate.GetProperty("plate_size")[0].GetInt32();
								int canvasHeight = texturePlate.GetProperty("plate_size")[1].GetInt32();
								if (textureId == "dyemap")
								{
									canvasWidth/=4;
									canvasHeight/=4;
								}
								var info = new SKImageInfo(canvasWidth, canvasHeight);

								SKPaint background = new SKPaint {
									Color = new SKColor(0x00,0x00,0x00,0x00)
								};
								SKPaint underTex = new SKPaint {
									Color = new SKColor(0x00,0x00,0x00,0x00)
								};

								canvasContainer canvasPlate = canvasPlates.get(texturePlateRef);
								if (canvasPlate == null) {
									canvas = SKSurface.Create(info);
									ctx = canvas.Canvas;

									ctx.DrawRect(0, 0, canvasWidth, canvasHeight, background);

									canvasPlate = new canvasContainer(
										texturePlateId,
										textureId,
										canvas
									);
									canvasPlates.AddItem(texturePlateRef, canvasPlate);
								}
								canvas = canvasPlate.canvas;
								ctx = canvas.Canvas;

								for (int p=0; p<texturePlate.GetProperty("texture_placements").GetArrayLength(); p++) {
									dynamic placement = texturePlate.GetProperty("texture_placements")[p];
									if (!renderTextures.ContainsKey(placement.GetProperty("texture_tag_name").GetString()))
									{
										Console.WriteLine("Missing plate texture detected. Skipping."); continue;
									}
									byte[] placementTexture = renderTextures[placement.GetProperty("texture_tag_name").GetString()];
									SKBitmap imageTex = SKBitmap.Decode(placementTexture);

									ctx.DrawRect(
										placement.GetProperty("position_x").GetInt32()*scale, (float)(placement.GetProperty("position_y").GetInt32()*scale),
										(float)(placement.GetProperty("texture_size_x").GetInt32()*scale), (float)(placement.GetProperty("texture_size_y").GetInt32()*scale),
										underTex
									);

									if (placementTexture == null) {
										Console.WriteLine("TextureNotLoaded"+placement.texture_tag_name);
										continue;
									}
									ctx.DrawBitmap(imageTex, placement.GetProperty("position_x").GetInt32(), placement.GetProperty("position_y").GetInt32());
								}
								using (var image = canvas.Snapshot())
								using (var data = image.Encode())
								using (var stream = File.OpenWrite(Path.Combine(OutLoc, $"{modelName}_{texturePlateRef}.png")))
								{
									// save the data to a stream
									data.SaveTo(stream);
								}
							}
						}
						else if (texturePlates.GetArrayLength() > 1) {
							Console.WriteLine("MultipleTexturePlates?");
						}
					}
					
					foreach (string textureName in renderTextures.Keys)
					{
						if (textureName=="texturePlates" || modelName.StartsWith("Male-")) continue;
						byte[] textureFile = renderTextures[textureName];
						string ext = "";
						if (textureFile[1] == 'P' && textureFile[2] == 'N' && textureFile[3] == 'G') ext = "png";
						else ext = "jpg";

						string directory = Path.Combine(OutLoc, "Textures", modelName.Replace("Female-", ""));
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
					foreach (dynamic tgxBin in renderRaws)
					{
						foreach (KeyValuePair<string,dynamic> file in tgxBin.files)
						{
							string directory = Path.Combine(OutLoc, "Raws", modelName);
							if (!Directory.Exists(directory)) 
							{
								Directory.CreateDirectory(directory);
							}
							if (file.Key == "render_metadata.js")
							{
								using (StreamWriter TGXWriter = File.CreateText(Path.Combine(directory, file.Key)))
								{
									TGXWriter.Write(file.Value.data);
								}
							}
							else
							{
								using (FileStream TGXWriter = new FileStream(Path.Combine(directory, file.Key), FileMode.Create, FileAccess.Write))
								{
									TGXWriter.Write(file.Value.data);
								}
							}
						}
					}
				}
			}
			
			if (riggedMeshes > 0)
			{
				sceneNodes.Add(libScenes.visual_scene[0].node[1]);
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

			// Save nodegen scripts
			foreach (KeyValuePair<string, string> kvp in ShaderPresets.scripts)
			{
				using (StreamWriter shaderWriter = new StreamWriter(Path.Combine(OutLoc, "Shaders", $"{Regex.Replace(kvp.Key, @"[^A-Za-z0-9\.]", "-")}.py")))
				{
					string filledShader = kvp.Value.Replace("OUTPUTPATH", Path.Combine(OutLoc, "Textures", Regex.Replace(kvp.Key, @"[^A-Za-z0-9\.]", "-"))).Replace("\\", "/");
					shaderWriter.Write(filledShader);
				}
			}
		}
	}
}