using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Collada141;

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
	
	public static void WriteFile(JArray renderMeshes, string writeLocation)
	{
		int fileNum = 0;
		while( File.Exists(writeLocation+@"\DestinyModel"+fileNum+@".dae") ) 
		{
			fileNum++;
		}
		string OutLoc = writeLocation+@"\DestinyModel"+fileNum+@".dae";

		COLLADA model = COLLADA.Load(@"Resources\template.dae");

		DateTime rightNow = DateTime.UtcNow;

		model.asset.created = rightNow;
		model.asset.modified = rightNow;
		model.asset.unit.meter = 1D;
		model.asset.unit.name = "meter";

		library_geometries libGeoms = model.Items[1] as library_geometries;
		List<geometry> geoms = new List<geometry>(libGeoms.geometry);
		
		library_controllers libControls = model.Items[2] as library_controllers;
		List<controller> controls = new List<controller>();
		controller controlTemplate = libControls.controller[0];
		
		library_visual_scenes libScenes = model.Items[3] as library_visual_scenes;
		List<node> sceneNodes = new List<node>();
		node nodeTemplate = libScenes.visual_scene[0].node[0];
		//List<node> riggedNodes = new List<node>();
		node riggedNodeTemplate = libScenes.visual_scene[0].node[2];//.node1[1];
		//node outSkeleton = libScenes.visual_scene[0].node[2].node1[0];
		//riggedNodes.Add(libScenes.visual_scene[0].node[1].node1[0]); //Place skeleton as the first node of the armature.

		int vertexOffset = 0;
		
		int riggedMeshes = 0;
		
		for (var m=0; m<renderMeshes.Count; m++) 
		{
			string mN = m.ToString("000");
			if (geoms.Count <= m) 
			{
				geoms.Add(geoms[0].Copy<geometry>());
			}
			geoms[m].id = "Model_"+mN+"-mesh";
			geoms[m].name = "Model."+mN;

			mesh meshObj = geoms[m].Item as mesh;
			
			bool doRigging = false;

			// Vertex positions
			meshObj.source[0].id = "Model_"+mN+"-mesh-positions";
			float_array vertPositions = meshObj.source[0].Item as float_array;
			vertPositions.id = "Model_"+mN+"-mesh-positions-array";
			meshObj.source[0].technique_common.accessor.source = "#Model_"+mN+"-mesh-positions-array";

			meshObj.vertices.id = "Model_"+mN+"-mesh-vertices";
			meshObj.vertices.input[0].source = "#Model_"+mN+"-mesh-positions";

			// First UV map
			meshObj.source[1].id = "Model_"+mN+"-mesh-map-0";
			float_array vertTexcoord0 = meshObj.source[1].Item as float_array;
			vertTexcoord0.id = "Model_"+mN+"-mesh-map-0-array";
			meshObj.source[1].technique_common.accessor.source = "#Model_"+mN+"-mesh-map-0-array";

			// Vertex normals
			meshObj.source[2].id = "Model_"+mN+"-mesh-normals";
			float_array vertNormals = meshObj.source[2].Item as float_array;
			vertNormals.id = "Model_"+mN+"-mesh-normals-array";
			meshObj.source[2].technique_common.accessor.source = "#Model_"+mN+"-mesh-normals-array";

			// Vertex tangents
			meshObj.source[3].id = "Model_"+mN+"-mesh-tangents";
			float_array vertTangents = meshObj.source[3].Item as float_array;
			vertTangents.id = "Model_"+mN+"-mesh-tangents-array";
			meshObj.source[3].technique_common.accessor.source = "#Model_"+mN+"-mesh-tangents-array";

			// Second UV map
			meshObj.source[4].id = "Model_"+mN+"-mesh-map-1";
			float_array vertTexcoord1 = meshObj.source[4].Item as float_array;
			vertTexcoord1.id = "Model_"+mN+"-mesh-map-1-array";
			meshObj.source[4].technique_common.accessor.source = "#Model_"+mN+"-mesh-map-1-array";

			// Vertex colors
			meshObj.source[5].id = "Model_"+mN+"-mesh-colors-Col";
			float_array vertColors = meshObj.source[5].Item as float_array;
			vertColors.id = "Model_"+mN+"-mesh-colors-Col-array";
			meshObj.source[5].technique_common.accessor.source = "#Model_"+mN+"-mesh-colors-Col-array";

			// Dye slots
			meshObj.source[6].id = "Model_"+mN+"-mesh-colors-slots";
			float_array vertSlots = meshObj.source[6].Item as float_array;
			vertSlots.id = "Model_"+mN+"-mesh-colors-slots-array";
			meshObj.source[6].technique_common.accessor.source = "#Model_"+mN+"-mesh-colors-slots-array";




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

			//if (m != 0) continue;
			//if (m != 1) continue;

			if (parts.Count == 0) {
				//Console.WriteLine("Skipped RenderMesh["+geometryHash+":"+m+"]: No parts");
				continue;
			} // Skip meshes with no parts

			//console.log('RenderMesh['+m+']', renderMesh);

			// Spasm.Renderable.prototype.render
			var partCount = -1;
			for (var p=0; p<parts.Count; p++) 
			{
				var part = parts[p];

				if (!checkRenderPart(part)) continue;

				// Ghost Shell Eye Bg
				//if (m != 0) continue;
				//if (p != 3) continue;

				//if (m != 0) continue;
				//if (p <6) continue;

				// Phoenix Strife Type 0 - Feathers
				//if (m != 1 && p != 1) continue;

				//Console.WriteLine("RenderMeshPart["+geometryHash+":"+m+":"+p+"]", part);
				partCount++;

				int gearDyeSlot = part.gearDyeSlot.Value;

				//if (gearDyeSlotOffsets[gearDyeSlot] == undefined) {
				//	console.warn('MissingDefaultDyeSlot', gearDyeSlot);
				//	gearDyeSlot = 0;
				//}
				//var materialIndex = gearDyeSlotOffsets[gearDyeSlot]+(part.usePrimaryColor ? 0 : 1);

				//console.log('RenderMeshPart['+geometryHash+':'+m+':'+p+']', part);

				// Load Material   CURRENTLY NOT SUPPORTING MATERIAL DATA
				//if (loadTextures) {
				//	var textures = geometryTextures[geometryHash];
				//	if (!textures) {
				//		//console.warn('NoGeometryTextures['+geometryHash+']', part);
				//	} else {
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

					for (var j=0; j<3; j++) 
					{
						int index = (int) indexBuffer[faceIndex+tri[j]].Value;
						dynamic vertex = vertexBuffer[index];
						if (vertex == null) { // Verona Mesh
							Console.WriteLine("MissingVertex["+index+"]");
							i=count;
							break;
						}
						double[] normal = new double[4] {(double)vertex.normal0[0].Value,(double)vertex.normal0[1].Value,(double)vertex.normal0[2].Value,(double)vertex.normal0[3].Value};
						double[] tangent = new double[4] {(double)vertex.tangent0[0].Value,(double)vertex.tangent0[1].Value,(double)vertex.tangent0[2].Value,(double)vertex.tangent0[3].Value};
						double[] uv = new double[2] {(double)vertex.texcoord0[0].Value,(double)vertex.texcoord0[1].Value};
						double[] color = new double[4];
						if (vertex.color0 != null) 
						{
							//color = {vertex.color0[0],vertex.color0[1],vertex.color0[2],vertex.color0[3]};
							colorArray.Add(vertex.color0[0].Value);
							colorArray.Add(vertex.color0[1].Value);
							colorArray.Add(vertex.color0[2].Value);
							colorArray.Add(vertex.color0[3].Value);
						}
						else 
						{
							//color = {0,0,0,0};
							colorArray.Add(0);
							colorArray.Add(0);
							colorArray.Add(0);
							colorArray.Add(0);
						}

						//slotArray.Add(part.gearDyeSlot.Value); // THIS NEEDS TO BE ADDED TO PARRAY
						parray.Append(index);
						parray.Append(' ');
						parray.Append(part.gearDyeSlot.Value);
						if (index<indexBuffer.Count-1) parray.Append(' ');

						double[] detailUv = vertex.texcoord2.ToObject<double[]>();
						if (vertex.texcoord2 == null) detailUv = new double[2] {0,0};

						//faceVertex.Add((double)(index+vertexOffset));
						//faceVertexNormals.Add(new float[3] {normal[0], normal[1], normal[2]});
						normalArray.Add(normal[1]);
						normalArray.Add(normal[0] * -1);
						normalArray.Add(normal[2]);

						tangentArray.Add(tangent[1]);
						tangentArray.Add(tangent[0] * -1);
						tangentArray.Add(tangent[2]);

						var uvu = uv[0]*texcoordScale[0].Value+texcoordOffset[0].Value;
						var uvv = uv[1]*texcoordScale[1].Value+texcoordOffset[1].Value;
						//faceVertexUvs.Add(new float[2] {uvu, uvv});
						texcoord0Array.Add(uvu);
						texcoord0Array.Add(uvv);
						
						if ((vertex.blendindices0 != null) || (vertex.position0[3] != 255)) doRigging = true;

						//if (color) {
						//	//console.log('Color['+m+':'+p+':'+i+':'+j+']', color);
						//	faceColors.Add(new float[3] {color[0], color[1], color[2]});
						//}

						//if (p == 10) {
						//	console.log('Vertex['+j+']', index, vertex);
						//}

						//console.log(
						//	uv[0]+','+uv[1],
						//	texcoordScale[0]+'x'+texcoordScale[1],
						//	texcoordOffset[0]+','+texcoordOffset[1],
						//	detailUv[0]+','+detailUv[1]
						//);

						/*detailVertexUvs.Add(new float[2] {
							uvu*detailUv[0],
							uvv*detailUv[1]
						});*/
						texcoord1Array.Add(uvu*detailUv[0]);
						texcoord1Array.Add(uvu*detailUv[1]);
					}
					//var face = new THREE.Face3(faceVertex[0], faceVertex[1], faceVertex[2], faceVertexNormals);
					//face.materialIndex = materialIndex;
					//if (faceColors.length > 0) face.vertexColors = faceColors;
					//geometry.faces.push(face);
					//geometry.faceVertexUvs[0].push(faceVertexUvs);

					//if (geometry.faceVertexUvs.length < 2) geometry.faceVertexUvs.push([]);
					//geometry.faceVertexUvs[1].push(detailVertexUvs);
				}
			}

			//return;
			
			
			controller control = controlTemplate.Copy<controller>();
			control.id = "Model_"+mN+"-skin";
			control.name = "Skin."+mN;
			skin skinItem = control.Item as skin;
			skinItem.source1 = "#Model_"+mN+"-mesh";	
			skinItem.joints.input[0].source = "#Model-"+mN+"-skin-joints";
			skinItem.joints.input[1].source = "#Model-"+mN+"-skin-bind_poses";
			skinItem.vertex_weights.count = (ulong) vertexBuffer.Count;
			skinItem.vertex_weights.input[0].source = "#Model-"+mN+"-skin-joints";
			skinItem.vertex_weights.input[1].source = "#Model-"+mN+"-skin-weights";
			StringBuilder vcountArray = new StringBuilder();
			StringBuilder varray = new StringBuilder();
			
			skinItem.source[0].id = "Model-"+mN+"-skin-joints";
			skinItem.source[0].technique_common.accessor.source = "#Model-"+mN+"-skin-joints-array";
			Name_array jointNames = skinItem.source[0].Item as Name_array;
			jointNames.id = "Model-"+mN+"-skin-joints-array";
			skinItem.source[0].Item = jointNames;
			
			skinItem.source[1].id = "Model-"+mN+"-skin-bind_poses";
			skinItem.source[1].technique_common.accessor.source = "#Model-"+mN+"-skin-bind_poses-array";
			float_array bindPoses = skinItem.source[1].Item as float_array;
			bindPoses.id = "Model-"+mN+"-skin-bind_poses-array";
			skinItem.source[1].Item = bindPoses;
			
			skinItem.source[2].id = "Model-"+mN+"-skin-weights";
			skinItem.source[2].technique_common.accessor.source = "#Model-"+mN+"-skin-weights-array";
			skinItem.source[2].technique_common.accessor.count = (ulong) vertexBuffer.Count * 4;
			skinItem.source[2].technique_common.accessor.stride = 1;
			float_array skinWeights = skinItem.source[2].Item as float_array;
			skinWeights.id = "Model-"+mN+"-skin-weights-array";
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
				//if (platform == "web") { // Ignored on mobile?
				//	x = x*positionScale[0].Value+positionOffset[0].Value;
				//	y = y*positionScale[1].Value+positionOffset[1].Value;
				//	z = z*positionScale[2].Value+positionOffset[2].Value;
				//}
				//geometry.vertices.push(new THREE.Vector3(x, y, z));
				positionArray.Add(x);
				positionArray.Add(y);
				positionArray.Add(z);

				if (doRigging)
				{
					// Set bone weights
					var boneIndex = position[3];//Math.abs((positionOffset[3] * 32767.0) + 0.01);
					//var bone = geometry.bones[boneIndex];

					double[] blendIndices = vertex.blendindices0 == null ? new double[]{(double)boneIndex, 255, 255, 255} : new double[] {(double)vertex.blendindices0[0],(double)vertex.blendindices0[1],(double)vertex.blendindices0[2],(double)vertex.blendindices0[3]};
					double[] blendWeights = vertex.blendweight0 == null ? new double[]{1, 0, 0, 0} : new double[] {(double)vertex.blendweight0[0],(double)vertex.blendweight0[1],(double)vertex.blendweight0[2],(double)vertex.blendweight0[3]};

					//int[] skinIndex = new int[]{0, 0, 0, 0};
					//double[] skinWeight = new int[]{0, 0, 0, 0};

					int vertIndices = 0;

					var totalWeights = 0.0;
					for (var w=0; w<blendIndices.Length; w++) {
						if (blendIndices[w] > 72) break;
						//skinIndex[w] = blendIndices[w];
						//skinWeight[w] = blendWeights[w];
						varray.Append(blendIndices[w]+" ");
						varray.Append((weightCount)+" ");
						weightsList.Add((double)blendWeights[w]);
						totalWeights += blendWeights[w]*255.0;
						weightCount += 1;
						vertIndices += 1;
					}

					vcountArray.Append(vertIndices+" ");

					//if (totalWeights != 255) console.error('MissingBoneWeight', 255-totalWeights, i, j);

					//geometry.skinIndices.push(new THREE.Vector4().fromArray(skinIndex));
					//geometry.skinWeights.push(new THREE.Vector4().fromArray(skinWeight));
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
				sceneNode.instance_controller[0].url = "#Model_"+mN+"-skin";
				sceneNode.instance_controller[0].name = "Skin."+mN;
				sceneNode.instance_controller[0].skeleton[0] = "#Armature_b_pedestal";
			}
			else{
				sceneNode = nodeTemplate.Copy<node>();
				sceneNode.instance_geometry[0].url = "#Model_"+mN+"-mesh";
				sceneNode.instance_geometry[0].name = "Model."+mN;
			}
			sceneNode.id = "Model_"+mN;
			sceneNode.name = "Model."+mN;

			if(doRigging)
			{
				controls.Add(control);
				//riggedNodes.Add(sceneNode);
				riggedMeshes += 1;
			}
			//else{
				sceneNodes.Add(sceneNode);
			//}

			vertexOffset += vertexBuffer.Count;






			vertPositions.Values = positionArray.ToArray();
			vertPositions.count = (ulong) positionArray.Count;
			vertTexcoord0.Values = texcoord0Array.ToArray();
			vertTexcoord0.count = (ulong) texcoord0Array.Count;
			vertNormals.Values = normalArray.ToArray();
			vertNormals.count = (ulong) normalArray.Count;
			vertTangents.Values = tangentArray.ToArray();
			vertTangents.count = (ulong) tangentArray.Count;
			vertTexcoord1.Values = texcoord1Array.ToArray();
			vertTexcoord1.count = (ulong) texcoord1Array.Count;
			vertColors.Values = colorArray.ToArray();
			vertColors.count = (ulong) colorArray.Count;
			//vertSlots.Values = slotArray.ToArray();
			//vertSlots.count = (ulong) slotArray.Count;







			meshObj.source[0].Item = vertPositions;
			//meshObj.source[0].Item.count = positionArray.Count;
			meshObj.source[0].technique_common.accessor.count = (ulong) positionArray.Count / 3;

			meshObj.source[1].Item = vertTexcoord0;
			//meshObj.source[1].Item.count = texcoord0Array.Count;
			meshObj.source[1].technique_common.accessor.count = (ulong) texcoord0Array.Count / 2;

			meshObj.source[2].Item = vertNormals;
			//meshObj.source[2].Item.count = normalArray.Count;
			meshObj.source[2].technique_common.accessor.count = (ulong) normalArray.Count / 3;

			meshObj.source[3].Item = vertTangents;
			//meshObj.source[3].Item.count = tangentArray.Count;
			meshObj.source[3].technique_common.accessor.count = (ulong) tangentArray.Count / 3;

			meshObj.source[4].Item = vertTexcoord1;
			//meshObj.source[4].Item.count = texcoord1Array.Count;
			meshObj.source[4].technique_common.accessor.count = (ulong) texcoord1Array.Count / 2;

			meshObj.source[5].Item = vertColors;
			//meshObj.source[5].Item.count = colorArray.Count;
			meshObj.source[5].technique_common.accessor.count = (ulong) colorArray.Count / 4;

			meshObj.source[6].Item = vertSlots;
			//meshObj.source[6].Item.count = slotArray.Count;
			//meshObj.source[6].technique_common.accessor.count = (ulong) slotArray.Count / 4;

			triangles meshTris = meshObj.Items[0] as triangles;
			meshTris.input[0].source = "#Model_"+mN+"-mesh-vertices";
			meshTris.input[1].source = "#Model_"+mN+"-mesh-map-0";
			meshTris.input[2].source = "#Model_"+mN+"-mesh-normals";
			meshTris.input[3].source = "#Model_"+mN+"-mesh-tangents";
			meshTris.input[4].source = "#Model_"+mN+"-mesh-map-1";
			meshTris.input[5].source = "#Model_"+mN+"-mesh-colors-Col";
			meshTris.input[6].source = "#Model_"+mN+"-mesh-colors-slots";
			meshTris.p = parray.ToString();
			meshObj.Items[0] = meshTris;

			geoms[m].Item = meshObj;
		}
		
		if (riggedMeshes > 0)
		{
			//libScenes.visual_scene[0].node[1].node1 = riggedNodes.ToArray();
			sceneNodes.Add(libScenes.visual_scene[0].node[1]);
		}

		libGeoms.geometry = geoms.ToArray();
		model.Items[1] = libGeoms;
		libControls.controller = controls.ToArray();
		model.Items[2] = libControls;
		libScenes.visual_scene[0].node = sceneNodes.ToArray();
		model.Items[3] = libScenes;

		model.Save(OutLoc);
	}
}
