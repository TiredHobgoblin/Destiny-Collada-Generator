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
		
		return shouldRender;
	}
	
	public static void WriteFile(JArray renderModels, string writeLocation)
	{
		int fileNum = 0;
		while( Directory.Exists(writeLocation+@"\DestinyModel"+fileNum) ) 
		{
			fileNum++;
		}
		string OutLoc = writeLocation+@"\DestinyModel"+fileNum;
		Directory.CreateDirectory(OutLoc);

		COLLADA model = COLLADA.Load(@"Resources\template.dae");

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
			modelName = Regex.Replace(modelName, @"[\s/\\]", "-");
			
			// Geometry
			for (var m=0; m<renderMeshes.Count; m++) 
			{
				string mN = m.ToString("000");

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
					int shader = (int)part.shader.type.Value;

					if (shader != 7) transparencyType = 24;
					//if ((flags & 0x8) != 0) transparencyType = 8;

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
							if (index<indexBuffer.Count-1) parray.Append(' ');

							vertexBuffer[index].dyeSlot0 = gearDyeSlot;

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
						case "texcoord":
							param pS = new param(); pS.name="S"; pS.type="float";
							param pT = new param(); pT.name="T"; pT.type="float";
							techniqueAccessor.param = new param[]{pS, pT};
							techniqueAccessor.stride = 2;
							break;
						case "color":
						case "dyeSlot":
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

					if (semName == "dyeSlot0")
					{
						valueArray.count = 128;
						valueArray._Text_ = "0.333 0 0 1   0.666 0 0 1   0.999 0 0 1   0 0.333 0 1   0 0.666 0 1   0 0.999 0 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 0.25 1   0.666 0 0.25 1   0.999 0 0.25 1   0 0.333 0.25 1   0 0.666 0.25 1   0 0.999 0.25 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 0.5 1   0.666 0 0.5 1   0.999 0 0.5 1   0 0.333 0.5 1   0 0.666 0.5 1   0 0.999 0.5 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1   0.333 0 1 1   0.666 0 1 1   0.999 0 1 1   0 0.333 1 1   0 0.666 1 1   0 0.999 1 1   0.750 0.750 0.750 1   0.750 0.750 0.750 1";
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
						if (eName == "dyeSlot0") continue;
						if (index == -1) {Console.WriteLine($"Vertex {v} has an element not found in vertex 0."); continue;}

						var eValues = vElement.Value;
						
						switch(Regex.Replace(eName, @"[0-9]", ""))
						{
							case "position":
							case "normal":
							case "tangent":
								semanticValues[index].Append($"{eValues[0]} {eValues[1]} {eValues[2]} ");
								break;
							case "texcoord":
								semanticValues[index].Append($"{eValues[0]} {eValues[1]} ");
								break;
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
							if (blendIndices[w] > 72) break;
							varray.Append(blendIndices[w]+" ");
							varray.Append((weightCount)+" ");
							weightsList.Add((double)blendWeights[w]);
							totalWeights += blendWeights[w]*255.0;
							weightCount += 1;
							vertIndices += 1;
						}

						if (vertex.dyeSlot0 != null) {varray.Append((vertex.dyeSlot0 + 72)+" ");}
						else {varray.Append((73)+" ");}
						varray.Append((weightCount)+" ");
						weightsList.Add(1.0);
						weightCount += 1;

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
					if (eName != "dyeSlot0")
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
							case "texcoord":
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
					int inSet = Int32.Parse(Regex.Replace(eName, @"[a-zA-Z]", ""));
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
						case "texcoord":
							triInput.semantic = "TEXCOORD";
							break;
						case "color":
							triInput.semantic = "COLOR";
							triInput.set += 1;
							break;
						case "dyeSlot":
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
							using (var image = canvas.Snapshot())
							using (var data = image.Encode())
							using (var stream = File.OpenWrite($@"{OutLoc}\{modelName}_{texturePlateRef}.png"))
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
					if (!Directory.Exists($@"{OutLoc}\Textures\{modelName}")) 
					{
						Directory.CreateDirectory($@"{OutLoc}\Textures\{modelName}");
					}
					using (FileStream texWriter = new FileStream($@"{OutLoc}\Textures\{modelName}\{textureName}.png", FileMode.Create, FileAccess.Write))
					{
						byte[] textureFile = renderTextures.Property(textureName).Value;
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

		model.Save(OutLoc+@"\model.dae");
	}
}
