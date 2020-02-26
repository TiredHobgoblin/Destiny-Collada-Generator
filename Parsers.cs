using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

// Methods used to parse large sections of the input
class Parsers
{
	public static JObject parseStagePart(JObject staticStagePart) {
		dynamic stagePart = staticStagePart;

		var gearDyeSlot = 0;
		var usePrimaryColor = true;
		var useInvestmentDecal = false;

		switch((int)stagePart.gear_dye_change_color_index.Value) {
			case 0:
				gearDyeSlot = 0;
				break;
			case 1:
				gearDyeSlot = 1;
				usePrimaryColor = false;
				break;
			case 2:
				gearDyeSlot = 2;
				break;
			case 3:
				gearDyeSlot = 3;
				usePrimaryColor = false;
				break;
			case 4:
				gearDyeSlot = 4;
				break;
			case 5:
				gearDyeSlot = 5;
				usePrimaryColor = false;
				break;
			case 6:
				gearDyeSlot = 6;
				useInvestmentDecal = true;
				break;
			case 7:
				gearDyeSlot = 7;
				useInvestmentDecal = true;
				break;
			default:
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("UnknownDyeChangeColorIndex["+stagePart.gear_dye_change_color_index+"]", stagePart);
				Console.ResetColor();
				break;
		}

		dynamic part = new JObject(
			//externalIdentifier: stagePart.external_identifier,
			//changeColorIndex: stagePart.gear_dye_change_color_index,
			//primitiveType: stagePart.primitive_type,
			//lodCategory: stagePart.lod_category,
			new JProperty("gearDyeSlot", gearDyeSlot),
			new JProperty("usePrimaryColor", usePrimaryColor),
			new JProperty("useInvestmentDecal", useInvestmentDecal)
			//indexMin: stagePart.index_min,
			//indexMax: stagePart.index_max,
			//indexStart: stagePart.start_index,
			//indexCount: stagePart.index_count
		);

		foreach (var keyP in stagePart) {
			var key = keyP.Name;
			var partKey = key;
			var value = stagePart.GetValue(key);
			switch(key) {
				//case 'external_identifier': partKey = 'externalIdentifier'; break;
				case "gear_dye_change_color_index": partKey = "changeColorIndex"; break;
				//case 'primitive_type': partKey = 'primitiveType'; break;
				//case 'lod_category': partKey = 'lodCategory'; break;

				//case 'index_min': partKey = 'indexMin'; break;
				//case 'index_max': partKey = 'indexMax'; break;
				case "start_index": partKey = "indexStart"; break;
				//case 'index_count': partKey = 'indexCount'; break;

				case "shader":
					var staticTextures = value.static_textures;
					value = new JObject(
						new JProperty("type", value.type)
					);
					if (staticTextures != null) value.staticTextures = staticTextures;
					break;

				//case 'static_textures': partKey = 'staticTextures'; break;

				default:
					string[] keyWords = key.Split("_");
					partKey = "";
					for (var i=0; i<keyWords.Length; i++) {
						var keyWord = keyWords[i];
						partKey += i == 0 ? keyWord : keyWord.Substring(0, 1).ToUpper()+keyWord.Substring(1);
					}
					break;
			}
			part[partKey] = value;
		}

		return part;
	}
	
	public static JArray parseVertexBuffers(JObject staticTgxBin, JObject staticRenderMesh)
	{
		dynamic tgxBin = staticTgxBin;
		dynamic renderMesh = staticRenderMesh;

		if (renderMesh.stage_part_vertex_stream_layout_definitions.Count > 1) {
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("Multiple Stage Part Vertex Layout Definitions", renderMesh.stage_part_vertex_stream_layout_definitions);
			Console.ResetColor();
		}
		dynamic stagePartVertexStreamLayoutDefinition = renderMesh.stage_part_vertex_stream_layout_definitions[0];
		dynamic formats = stagePartVertexStreamLayoutDefinition.formats;

		dynamic vertexBuffer = new JArray();

		for (var vertexBufferIndex = 0; vertexBufferIndex < renderMesh.vertex_buffers.Count; vertexBufferIndex++) {
			dynamic vertexBufferInfo = renderMesh.vertex_buffers[vertexBufferIndex];
			byte[] vertexBufferData = tgxBin.files[tgxBin.lookup.IndexOf(tgxBin.SelectToken(string.Format("lookup[?(@ == '{0}')]", vertexBufferInfo.file_name)))].data;
			dynamic format = formats[vertexBufferIndex];

			var vertexIndex = 0;
			for (var v=0; v<vertexBufferInfo.byte_size.Value; v+= vertexBufferInfo.stride_byte_size.Value) {
				var vertexOffset = v;
				if (vertexBuffer.Count <= vertexIndex) vertexBuffer.Add(new JObject());//vertexBuffer[vertexIndex] = new JObject();
				for (var e=0; e<format.elements.Count; e++) {
					dynamic element = format.elements[e];
					dynamic values = new JArray();

					string elementType = element.type.Value.Replace("_vertex_format_attribute_", "");
					string[] types = new string[] {"ubyte", "byte", "ushort", "short", "uint", "int", "float"};
					foreach (var typeIndex in types) {
						string type = typeIndex;
						if (elementType.IndexOf(type) == 0) {
							var count = Convert.ToInt32(elementType.Replace(type, ""));
							var j = 0;
							dynamic value;
							switch(type) {
								case "ubyte":
									for (j=0; j<count; j++) {
										value = vertexBufferData[vertexOffset];
										if (element.normalized.Value) value = TGXMUtils.unormalize(value, 8);
										values.Add(value);
										vertexOffset++;
									}
									break;
								case "byte":
									for (j=0; j<count; j++) {
										value = TGXMUtils.Sbyte(vertexBufferData, vertexOffset);
										if (element.normalized.Value) value = TGXMUtils.normalize(value, 8);
										values.Add(value);
										vertexOffset++;
									}
									break;
								case "ushort":
									for(j=0; j<count; j++) {
										value = BitConverter.ToUInt16(vertexBufferData, vertexOffset);
										if (element.normalized.Value) value = TGXMUtils.unormalize(value, 16);
										values.Add(value);
										vertexOffset += 2;
									}
									break;
								case "short":
									for(j=0; j<count; j++) {
										value = BitConverter.ToInt16(vertexBufferData, vertexOffset);
										if (element.normalized.Value) value = TGXMUtils.normalize(value, 16);
										values.Add(value);
										vertexOffset += 2;
									}
									break;
								case "uint":
									for(j=0; j<count; j++) {
										value = BitConverter.ToUInt32(vertexBufferData, vertexOffset);
										if (element.normalized.Value) value = TGXMUtils.unormalize(value, 32);
										values.Add(value);
										vertexOffset += 4;
									}
									break;
								case "int":
									for(j=0; j<count; j++) {
										value = BitConverter.ToInt32(vertexBufferData, vertexOffset);
										if (element.normalized.Value) value = TGXMUtils.normalize(value, 32);
										values.Add(value);
										vertexOffset += 4;
									}
									break;
								case "float":
									for(j=0; j<count; j++) {
										value = BitConverter.ToSingle(vertexBufferData, vertexOffset);
										values.Add(value);
										vertexOffset += 4;
									}
									break;
							}
							break;
						}
					}

					string semantic = element.semantic.Value.Replace("_tfx_vb_semantic_", "");
					switch(semantic) {
						case "position":
						case "normal":
						case "tangent":
						case "blendweight": // Bone weights 0-1
						case "blendindices": // Bone indices, 255=none, index starts at 1?
						case "color":
							break;
						case "texcoord":
							semantic = "uv";
							break;
						default:
							Console.WriteLine($"Unknown Vertex Semantic : {semantic} : {element.semantic_index}");
							break;
					}
					int semantic_index = (int) element.semantic_index.Value;
					if (semantic_index != 0) semantic_index--;
					vertexBuffer[vertexIndex].Add(new JProperty(semantic+semantic_index, values));
				}
				vertexIndex++;
			}
		}
		return vertexBuffer;
	}
	
	public static JArray parseTGXAsset(JObject staticTgxBin) 
	{
		Console.WriteLine("Parsing model data...");
		dynamic tgxBin = staticTgxBin;
		
		dynamic metadata = new JObject(tgxBin.metadata); // Arrangement

		dynamic meshes = new JArray();

		for (var r=0; r<metadata.render_model.render_meshes.Count; r++) {
			Console.WriteLine("Parsing object "+r+"...");

			var renderMeshIndex = r;
			dynamic renderMesh = metadata.render_model.render_meshes[renderMeshIndex]; // BoB Bunch of Bits


			// IndexBuffer
			Console.Write("Parsing object "+r+" index buffer... ");
			var indexBufferInfo = renderMesh.index_buffer;
			byte[] indexBufferData = tgxBin.files[tgxBin.lookup.IndexOf(tgxBin.SelectToken(string.Format("lookup[?(@ == '{0}')]", indexBufferInfo.file_name)))].data;

			dynamic indexBuffer = new JArray();
			for (var j=0; j<indexBufferInfo.byte_size.Value; j+=indexBufferInfo.value_byte_size.Value) {
				var indexValue = BitConverter.ToUInt16(indexBufferData, j);
				indexBuffer.Add(indexValue);
			}
			//console.log('IndexBuffer', indexBufferInfo);
			Console.WriteLine("Done.");

			// VertexBuffer
			Console.Write("Parsing object "+r+" vertex buffers... ");
			dynamic vertexBuffer = parseVertexBuffers(staticTgxBin, renderMesh);
			Console.WriteLine("Done.");

			dynamic parts = new JArray();
			List<int> partIndexList = new List<int>();
			int[] stagesToRender = {0, 7, 15}; // Hardcoded?
			dynamic partOffsets = new JArray();

			Console.Write("Parsing object "+r+" stage parts... ");
			foreach (var stagePart in renderMesh.stage_part_list) {
				//if (!stagePart) { NOT YET SURE HOW TO SKIP NONEXISTENT PARTS
				//    //console.warn('MissingStagePart['+renderMeshIndex+':'+partOffset+']');
				//    Console.WriteLine("MissingStagePart["+renderMeshIndex+":"+partOffset+"]");
				//    continue;
				//}

				if (partIndexList.IndexOf((int)stagePart.start_index.Value) != -1) {
					// Skip duplicate parts
					continue;
				}
				partIndexList.Add((int)stagePart.start_index.Value);
				parts.Add(parseStagePart(stagePart));
			}
			Console.WriteLine("Done.");

			// Spasm.RenderMesh
			meshes.Add(new JObject(
				new JProperty("positionOffset", renderMesh.position_offset),
				new JProperty("positionScale", renderMesh.position_scale),
				new JProperty("texcoordOffset", renderMesh.texcoord_offset),
				new JProperty("texcoordScale", renderMesh.texcoord_scale),
				new JProperty("texcoord0ScaleOffset", renderMesh.texcoord0_scale_offset),
				new JProperty("indexBuffer", indexBuffer),
				new JProperty("vertexBuffer", vertexBuffer),
				new JProperty("parts", parts)
			));
		}

		Console.WriteLine("Model data parsed.");
		return meshes;
	}
}