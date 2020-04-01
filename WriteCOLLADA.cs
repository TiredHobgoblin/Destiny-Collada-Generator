using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using Collada141;

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
	public static bool checkRenderPart(JObject staticPart) {
		bool shouldRender = false;

		dynamic part = staticPart;
		
		string lodCategoryName = part.lodCategory.name.Value;
		
		if (lodCategoryName.IndexOf('0') >= 0) shouldRender = true;
		if (lodCategoryName.IndexOf("unused") >= 0) shouldRender = true;
		if (lodCategoryName.IndexOf("count") >= 0) shouldRender = true;
		
		return shouldRender;
	}
	
	public static void WriteFile(JArray renderModels, string writeLocation, string game)
	{
		// D2 uses some different values than D1. Game-dependent values will be assigned here.
		int defaultShader = 9;

		if (game == "2")
		{
			defaultShader = 7;
		}
		
		int fileNum = 0;
		while( Directory.Exists(Path.Combine(new string[]{writeLocation, "DestinyModel", fileNum.ToString()})) ) 
		{
			fileNum++;
		}
		string OutLoc = Path.Combine(writeLocation, "DestinyModel", fileNum.ToString());
		Directory.CreateDirectory(OutLoc);

		COLLADA model = COLLADA.Load(Path.Combine("Resources", "template.dae"));

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
			JArray renderMeshes = renderModel.meshes;
			dynamic renderTextures = renderModel.textures;
			string modelName = renderModel.name;
			modelName = Regex.Replace(modelName, @"[^A-Za-z0-9\.]", "-");

			// Geometry
			for (var m=0; m<renderMeshes.Count; m++) 
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
					continue;
				} // Skip meshes with no parts


				// Spasm.Renderable.prototype.render
				var partCount = -1;
				foreach (var part in parts)
				{
					if (!checkRenderPart(part)) continue;

					partCount++;

					int gearDyeSlot = part.gearDyeSlot.Value;
					int transparencyType = 0;

					int flags = (int)part.flags.Value;
					int shader = defaultShader;
					int variant = (int)part.variantShaderIndex;
					if (part.shader != null) shader = (int)part.shader.type.Value;
					//else if (part.variantShaderIndex != -1) shader = -1;

					if (shader != defaultShader) transparencyType = 24;
					if (shader == -1) transparencyType = 32;
					
					JArray shaderCoord = new JArray();
					shaderCoord.Add(shader/10.0+0.05);
					shaderCoord.Add(1-(variant/10.0+0.15));
					if (game == "") // Check for known D1 shaders
					{
						switch (shader)
						{
							case -1: // Unknown. Sight?
								if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses unknown-shader-1.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							case 1: // Unknown emissive.
								if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses unknown-emissive-1.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							case 7: // Decal.
								if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses decal.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							case 8: // Controlled emissive.
								if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses controlled emissive.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							case 9: // Default.
								if (variant==-1) {}
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							case 11: // Emissive decal.
								if (variant==-1) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses emissive decal.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
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
							case 7: // Default. Variants: 
								if (variant==-1) {}
								else if (variant!=2) Console.WriteLine($"Mesh {m} part {partCount} in {modelName} uses default with transmission.");
								else Console.WriteLine($"Unknown variant {variant} in mesh {m} part {partCount}.");
								break;
							default:
								Console.WriteLine($"Unknown shader {shader} in {modelName} mesh {m} part {partCount}.");
								break;
						}
					}
					if ((flags & 0x8) != 0) transparencyType = 8;

					// Load Vertex Stream
					int increment = 3;
					int start = (int)part.indexStart.Value;
					int count = (int)part.indexCount.Value;

					// PrimitiveType, 3=TRIANGLES, 5=TRIANGLE_STRIP
					// https://stackoverflow.com/questions/3485034/convert-triangle-strips-to-triangles

					if (part.primitiveType.Value == 5) {
						increment = 1;
						count -= 2;
					}

					for (int i=0; i<count; i+= increment) 
					{
						List<double[]> faceVertexNormals = new List<double[]>();
						List<double[]> faceVertexUvs = new List<double[]>();
						List<double[]> faceVertex = new List<double[]>();

						List<double[]> faceColors = new List<double[]>();

						List<double[]> detailVertexUvs = new List<double[]>();

						int faceIndex = start+i;

						int[] tri = ((int)part.primitiveType.Value) == 3 || ((i & 1) != 0) ? new int[3]{0, 1, 2} : new int[3]{2, 1, 0};

						if (indexBuffer[faceIndex+0] == 65535 || indexBuffer[faceIndex+1] == 65535 || indexBuffer[faceIndex+2] == 65535) continue;

						for (var j=0; j<3; j++) 
						{
							int index = (int) indexBuffer[faceIndex+tri[j]].Value;
							dynamic vertex = vertexBuffer[index];

							if (vertex == null) { // Verona Mesh
								Console.WriteLine("MissingVertex["+index+"]");
								i=count;
								break;
							}

							parray.Append(index);
							parray.Append(' ');
							parray.Append(gearDyeSlot+transparencyType);
							parray.Append(' ');

							vertexBuffer[index].slots = gearDyeSlot;
							//vertexBuffer[index].shader0 = shaderCoord;
						}
					}
				}

				controller control = controlTemplate.Copy<controller>();
				control.id = modelName+"_"+mN+"-skin";
				control.name = modelName+"_Skin."+mN;
				skin skinItem = control.Item as skin;
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

				if (vertexBuffer[0].slots == null) vertexBuffer[0].slots = 0;
				//if (vertexBuffer[0].shader0 == null){
				//	vertexBuffer[0].shader0 = new JArray();
				//	vertexBuffer[0].shader0.Add(0);
				//	vertexBuffer[0].shader0.Add(0);
				//}

				foreach (JProperty vSemantic in vertexBuffer[0].Properties())
				{
					string semName = vSemantic.Name;
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
					var position = vertex.position0;

					foreach (JProperty vElement in vertex.Properties())
					{
						string eName = vElement.Name;
						int index = semanticNames.IndexOf(eName);
						if (eName == "slots" || eName == "blendweight0" || eName == "blendindices0") continue;
						if (index == -1) {Console.WriteLine($"Vertex {v} has an element not found in vertex 0."); continue;}

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
								float texcoordX = vertex.uv0[0]*texcoordScale[0]+texcoordOffset[0];
								float texcoordY = vertex.uv0[1]*texcoordScale[1]+texcoordOffset[1];
								if (eName != "uv0")
								{
									texcoordX *= (float) eValues[0];
									texcoordY *= (float) eValues[1];
								}
								semanticValues[index].Append($"{texcoordX} {1-texcoordY} ");
								break;
							//case "shader":
							//	semanticValues[index].Append($"{eValues[0]} {eValues[1]} ");
							//	break;
							case "color":
								semanticValues[index].Append($"{eValues[0]} {eValues[1]} {eValues[2]} {eValues[3]} ");
								break;
							default: break;
						}

						semanticCounts[index]++;
					}

					if ((vertex.blendindices0 != null) || (vertex.position0[3] != 255)) doRigging = true;
					
					if (doRigging)
					{
						// Set bone weights
						var boneIndex = position[3];//Math.abs((positionOffset[3] * 32767.0) + 0.01);

						double[] blendIndices = vertex.blendindices0 == null ? new double[]{(double)boneIndex, 255, 255, 255} : new double[] {(double)vertex.blendindices0[0],(double)vertex.blendindices0[1],(double)vertex.blendindices0[2],(double)vertex.blendindices0[3]};
						double[] blendWeights = vertex.blendweight0 == null ? new double[]{1, 0, 0, 0} : new double[] {(double)vertex.blendweight0[0],(double)vertex.blendweight0[1],(double)vertex.blendweight0[2],(double)vertex.blendweight0[3]};

						int vertIndices = 1;

						var totalWeights = 0.0;
						for (var w=0; w<blendIndices.Length; w++) {
							var blendIndex = blendIndices[w];
							if (blendIndex%1 != 0) blendIndex = Math.Floor(blendIndex);
							if (blendIndex > 72) break;
							varray.Append(blendIndex+" ");
							varray.Append((weightCount)+" ");
							weightsList.Add((double)blendWeights[w]);
							totalWeights += blendWeights[w]*255.0;
							weightCount += 1;
							vertIndices += 1;
						}

						if (vertex.slots != null) {varray.Append((vertex.slots + 72)+" ");}
						else {varray.Append((73)+" ");}
						varray.Append((/*weightCount*/ 0)+" ");
						//weightsList.Add(1.0);
						//weightCount += 1;

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

			// Textures
			if (renderTextures != null)
			{
				canvasArray canvasPlates = new canvasArray();
				SKSurface canvas;
				SKCanvas ctx;
				JArray plateMetas = renderTextures.texturePlates;
				foreach (JArray texturePlates in plateMetas)
				{
					if (texturePlates.Count == 1) {
						dynamic texturePlate = texturePlates[0];
						dynamic texturePlateSet = texturePlate.plate_set;

						// Stitch together plate sets
						// Web versions are pre-stitched

						foreach (var texturePlateProp in texturePlateSet.Properties()) {
							texturePlate = texturePlateProp.Value;
							string texturePlateId = texturePlateProp.Name;
							string texturePlateRef = texturePlateId+"_"+texturePlate.plate_index;

							string textureId = texturePlateId;
							switch(texturePlateId) {
								case ("diffuse"): textureId = "map"; break;
								case ("normal"): textureId = "normalMap"; break;
								case ("gearstack"): textureId = "gearstackMap"; break;
								default:
									Console.WriteLine("UnknownTexturePlateId: "+texturePlateId);
									break;
							}

							int scale = 1;

							//if (texturePlate.texture_placements.Count == 0) {
							//	continue;
							//}

							int canvasWidth = texturePlate.plate_size[0];
							int canvasHeight = texturePlate.plate_size[1];
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

							for (int p=0; p<texturePlate.texture_placements.Count; p++) {
								dynamic placement = texturePlate.texture_placements[p];
								byte[] placementTexture = renderTextures.Property(placement.texture_tag_name.Value).Value;
								SKBitmap imageTex = SKBitmap.Decode(placementTexture);

								ctx.DrawRect(
									placement.position_x.Value*scale, placement.position_y.Value*scale,
									placement.texture_size_x.Value*scale, placement.texture_size_y.Value*scale,
									underTex
								);

								if (placementTexture == null) {
									Console.WriteLine("TextureNotLoaded"+placement.texture_tag_name);
									continue;
								}
								ctx.DrawBitmap(imageTex, placement.position_x.Value, placement.position_y.Value);
							}

							string path = Path.Combine(new string[]{$"{OutLoc}", $"{modelName}_{texturePlateRef}.png"});
							using (var image = canvas.Snapshot())
							using (var data = image.Encode())
							using (var stream = File.OpenWrite(path))
							{
								// save the data to a stream
								data.SaveTo(stream);
							}
						}
					}
					else if (texturePlates.Count > 1) {
						Console.WriteLine("MultipleTexturePlates?");
					}
				}
				
				foreach (string textureName in renderTextures.names)
				{
					byte[] textureFile = renderTextures.Property(textureName).Value;
					string ext = "";
					if (textureFile[1] == 'P' && textureFile[2] == 'N' && textureFile[3] == 'G') ext = "png";
					else ext = "jpg";

					string directory = Path.Combine(new string[]{$"{OutLoc}", "Textures", $"{modelName}"});
					if (!Directory.Exists(directory)) 
					{
						Directory.CreateDirectory(directory);
					}
					using (FileStream texWriter = new FileStream(Path.Combine(new string[]{$"{directory}", $"{textureName}.{ext}"}), FileMode.Create, FileAccess.Write))
					{
						texWriter.Write(textureFile);
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
	}
}
