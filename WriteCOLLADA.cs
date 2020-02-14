using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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

				// Vertex positions
				meshObj.source[0].id = modelName+"_"+mN+"-mesh-positions";
				float_array vertPositions = meshObj.source[0].Item as float_array;
				vertPositions.id = modelName+"_"+mN+"-mesh-positions-array";
				meshObj.source[0].technique_common.accessor.source = "#Model_"+mN+"-mesh-positions-array";

				meshObj.vertices.id = modelName+"_"+mN+"-mesh-vertices";
				meshObj.vertices.input[0].source = "#"+modelName+"_"+mN+"-mesh-positions";

				// First UV map
				float_array vertTexcoord0 = meshObj.source[1].Item as float_array;

				// Vertex normals
				meshObj.source[2].id = modelName+"_"+mN+"-mesh-normals";
				float_array vertNormals = meshObj.source[2].Item as float_array;
				vertNormals.id = modelName+"_"+mN+"-mesh-normals-array";
				meshObj.source[2].technique_common.accessor.source = "#"+modelName+"_"+mN+"-mesh-normals-array";

				// Vertex tangents
				meshObj.source[3].id = modelName+"_"+mN+"-mesh-tangents";
				float_array vertTangents = meshObj.source[3].Item as float_array;
				vertTangents.id = modelName+"_"+mN+"-mesh-tangents-array";
				meshObj.source[3].technique_common.accessor.source = "#"+modelName+"_"+mN+"-mesh-tangents-array";

				// Second UV map
				float_array vertTexcoord1 = meshObj.source[4].Item as float_array;

				// Dye slots
				meshObj.source[5].id = modelName+"_"+mN+"-mesh-colors-slots";
				float_array vertSlots = meshObj.source[5].Item as float_array;
				vertSlots.id = modelName+"_"+mN+"-mesh-colors-slots-array";
				meshObj.source[5].technique_common.accessor.source = "#"+modelName+"_"+mN+"-mesh-colors-slots-array";

				// Vertex colors
				meshObj.source[6].id = modelName+"_"+mN+"-mesh-colors-Col";
				float_array vertColors = meshObj.source[6].Item as float_array;
				vertColors.id = modelName+"_"+mN+"-mesh-colors-Col-array";
				meshObj.source[6].technique_common.accessor.source = "#"+modelName+"_"+mN+"-mesh-colors-Col-array";






				// Dynamic arrays for staging the data
				List<double> positionArray = new List<double>();
				List<double> texcoord0Array = new List<double>();
				List<double> normalArray = new List<double>();
				List<double> tangentArray = new List<double>();
				List<double> texcoord1Array = new List<double>();
				List<double> colorArray = new List<double>();
				//List<double> slotArray = new List<double>();
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

					// Load Material   CURRENTLY NOT SUPPORTING MATERIAL DATA
					//if (loadTextures) 
					//{
					//	var textures = geometryTextures[geometryHash];
					//	if (!textures) 
					//	{
					//		//console.warn('NoGeometryTextures['+geometryHash+']', part);
					//	} 
					//	else 
					//	{
					//		//continue;
					//	}
					//	var material = parseMaterial(part, gearDyes[gearDyeSlot], textures);
					//
					//	if (material) {
					//		material.name = geometryHash+'-CustomShader'+m+'-'+p;
					//		materials.push(material);
					//		materialIndex = materials.length-1;
					//		//console.log('MaterialName['+materialIndex+']:'+material.name);
					//	}
					//}

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
				
				for (var v=0; v<vertexBuffer.Count; v++) 
				{
					dynamic vertex = vertexBuffer[v];
					var position = vertex.position0;
					float x = position[1].Value;//*positionScale[0]+positionOffset[0];
					float y = position[0].Value * -1;//*positionScale[1]+positionOffset[1];
					float z = position[2].Value;//*positionScale[2]+positionOffset[2]; // Apply negative scale to fix lighting
					positionArray.Add(x);
					positionArray.Add(y);
					positionArray.Add(z);

					double[] normal = new double[4] {(double)vertex.normal0[0].Value,(double)vertex.normal0[1].Value,(double)vertex.normal0[2].Value,(double)vertex.normal0[3].Value};
					double[] tangent = new double[4] {(double)vertex.tangent0[0].Value,(double)vertex.tangent0[1].Value,(double)vertex.tangent0[2].Value,(double)vertex.tangent0[3].Value};
					double[] uv = new double[2] {(double)vertex.texcoord0[0].Value,(double)vertex.texcoord0[1].Value};
					double[] color = new double[4];
					if (vertex.color0 != null) 
					{
						colorArray.Add(vertex.color0[0].Value);
						colorArray.Add(vertex.color0[1].Value);
						colorArray.Add(vertex.color0[2].Value);
						colorArray.Add(vertex.color0[3].Value);
					}
					else 
					{
						colorArray.Add(0);
						colorArray.Add(0);
						colorArray.Add(0);
						colorArray.Add(0);
					}

					double[] detailUv;
					if (vertex.texcoord2 == null) detailUv = new double[2] {0,0};
					else detailUv = new double[2] {(double)vertex.texcoord2[0], (double)vertex.texcoord2[1]};

					normalArray.Add(normal[0]);
					normalArray.Add(normal[1]);
					normalArray.Add(normal[2]);

					tangentArray.Add(tangent[0]);
					tangentArray.Add(tangent[1]);
					tangentArray.Add(tangent[2]);

					var uvu = uv[0]*texcoordScale[0].Value+texcoordOffset[0].Value;
					var uvv = uv[1]*texcoordScale[1].Value+texcoordOffset[1].Value;
					texcoord0Array.Add(uvu);
					texcoord0Array.Add(1-uvv);
					
					if ((vertex.blendindices0 != null) || (vertex.position0[3] != 255)) doRigging = true;

					texcoord1Array.Add(uvu*detailUv[0]);
					texcoord1Array.Add(1-(uvv*detailUv[1]));

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






				vertPositions.Values = positionArray.ToArray();
				vertPositions.count = (ulong) positionArray.Count;
				vertTexcoord0.Values = texcoord0Array.ToArray();
				vertTexcoord0.count = (ulong) texcoord0Array.Count;
				vertNormals.Values = normalArray.ToArray();
				vertNormals.count = (ulong) normalArray.Count;
				vertTangents.Values = tangentArray.ToArray();
				vertTangents.count = (ulong) tangentArray.Count;
				vertColors.Values = colorArray.ToArray();
				vertColors.count = (ulong) colorArray.Count;
				vertTexcoord1.Values = texcoord1Array.ToArray();
				vertTexcoord1.count = (ulong) texcoord1Array.Count;







				meshObj.source[0].Item = vertPositions;
				meshObj.source[0].technique_common.accessor.count = (ulong) positionArray.Count / 3;

				meshObj.source[1].Item = vertTexcoord0;
				meshObj.source[1].technique_common.accessor.count = (ulong) texcoord0Array.Count / 2;

				meshObj.source[2].Item = vertNormals;
				meshObj.source[2].technique_common.accessor.count = (ulong) normalArray.Count / 3;

				meshObj.source[3].Item = vertTangents;
				meshObj.source[3].technique_common.accessor.count = (ulong) tangentArray.Count / 3;

				meshObj.source[4].Item = vertTexcoord1;
				meshObj.source[4].technique_common.accessor.count = (ulong) texcoord1Array.Count / 2;

				meshObj.source[5].Item = vertSlots;

				meshObj.source[6].Item = vertColors;
				meshObj.source[6].technique_common.accessor.count = (ulong) colorArray.Count / 4;

				triangles meshTris = meshObj.Items[0] as triangles;
				meshTris.input[0].source = "#"+modelName+"_"+mN+"-mesh-vertices";
				//meshTris.input[1].source = "#uv0";
				meshTris.input[2].source = "#"+modelName+"_"+mN+"-mesh-normals";
				meshTris.input[3].source = "#Model_"+mN+"-mesh-tangents";
				//meshTris.input[4].source = "#uv1";
				meshTris.input[5].source = "#"+modelName+"_"+mN+"-mesh-colors-slots";
				meshTris.input[6].source = "#"+modelName+"_"+mN+"-mesh-colors-Col";
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
