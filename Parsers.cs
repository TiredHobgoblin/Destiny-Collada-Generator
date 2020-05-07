using System;
using System.Text.Json;
using System.Dynamic;
using System.Collections.Generic;

// Methods used to parse large sections of the input
class Parsers
{
	public static Dictionary<string,dynamic> parseStagePart(dynamic staticStagePart) {
		dynamic stagePart = staticStagePart;

		var gearDyeSlot = 0;
		var usePrimaryColor = true;
		var useInvestmentDecal = false;

		switch(stagePart.GetProperty("gear_dye_change_color_index").GetInt32()) {
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
				Console.WriteLine("UnknownDyeChangeColorIndex["+stagePart.GetProperty("gear_dye_change_color_index")+"]", stagePart);
				Console.ResetColor();
				break;
		}

		dynamic part = new Dictionary<string,dynamic>();
			//externalIdentifier: stagePart.external_identifier,
			//changeColorIndex: stagePart.gear_dye_change_color_index,
			//primitiveType: stagePart.primitive_type,
			//lodCategory: stagePart.lod_category,
			part.Add("gearDyeSlot", gearDyeSlot);
			part.Add("usePrimaryColor", usePrimaryColor);
			part.Add("useInvestmentDecal", useInvestmentDecal);
			//indexMin: stagePart.index_min,
			//indexMax: stagePart.index_max,
			//indexStart: stagePart.start_index,
			//indexCount: stagePart.index_count

		//foreach (var keyP in stagePart) {
		JsonElement.ObjectEnumerator stageProps = stagePart.EnumerateObject();
		while (stageProps.MoveNext())
		{
			var keyP = stageProps.Current;
			var key = keyP.Name;
			var partKey = key;
			dynamic value = keyP.Value;//stagePart.GetValue(key);
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
					bool hasStaticTextures = value.TryGetProperty("static_textures", out JsonElement staticTextures);
					var valueType=value.GetProperty("type");
					value = new ExpandoObject();
						value.type=valueType;
					if (hasStaticTextures) value.Add("staticTextures", staticTextures);
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
	
	public static List<Dictionary<string,dynamic>> parseVertexBuffers(ExpandoObject staticTgxBin, dynamic staticRenderMesh)
	{
		dynamic tgxBin = staticTgxBin;
		dynamic renderMesh = staticRenderMesh;

		if (renderMesh.GetProperty("stage_part_vertex_stream_layout_definitions").GetArrayLength() > 1) {
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("Multiple Stage Part Vertex Layout Definitions", renderMesh.GetProperty("stage_part_vertex_stream_layout_definitions"));
			Console.ResetColor();
		}
		dynamic stagePartVertexStreamLayoutDefinition = renderMesh.GetProperty("stage_part_vertex_stream_layout_definitions")[0];
		dynamic formats = stagePartVertexStreamLayoutDefinition.GetProperty("formats");

		List<Dictionary<string,dynamic>> vertexBuffer = new List<Dictionary<string,dynamic>>();

		for (var vertexBufferIndex = 0; vertexBufferIndex < renderMesh.GetProperty("vertex_buffers").GetArrayLength(); vertexBufferIndex++) {
			dynamic vertexBufferInfo = renderMesh.GetProperty("vertex_buffers")[vertexBufferIndex];
			byte[] vertexBufferData = tgxBin.files[vertexBufferInfo.GetProperty("file_name").GetString()].data;
			dynamic format = formats[vertexBufferIndex];

			var vertexIndex = 0;
			for (var v=0; v<vertexBufferInfo.GetProperty("byte_size").GetInt32(); v+= vertexBufferInfo.GetProperty("stride_byte_size").GetInt32()) {
				var vertexOffset = v;
				if (vertexBuffer.Count <= vertexIndex) vertexBuffer.Add(new Dictionary<string,dynamic>());//vertexBuffer[vertexIndex] = new JObject();
				for (var e=0; e<format.GetProperty("elements").GetArrayLength(); e++) {
					dynamic element = format.GetProperty("elements")[e];
					List<dynamic> values = new List<dynamic>();

					string elementType = element.GetProperty("type").GetString().Replace("_vertex_format_attribute_", "");
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
										if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.unormalize(value, 8);
										values.Add(value);
										vertexOffset++;
									}
									break;
								case "byte":
									for (j=0; j<count; j++) {
										value = TGXMUtils.Sbyte(vertexBufferData, vertexOffset);
										if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 8);
										values.Add(value);
										vertexOffset++;
									}
									break;
								case "ushort":
									for(j=0; j<count; j++) {
										value = BitConverter.ToUInt16(vertexBufferData, vertexOffset);
										if (element.GetProperty("normalized").GetBoolean) value = TGXMUtils.unormalize(value, 16);
										values.Add(value);
										vertexOffset += 2;
									}
									break;
								case "short":
									for(j=0; j<count; j++) {
										value = BitConverter.ToInt16(vertexBufferData, vertexOffset);
										if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 16);
										values.Add(value);
										vertexOffset += 2;
									}
									break;
								case "uint":
									for(j=0; j<count; j++) {
										value = BitConverter.ToUInt32(vertexBufferData, vertexOffset);
										if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.unormalize(value, 32);
										values.Add(value);
										vertexOffset += 4;
									}
									break;
								case "int":
									for(j=0; j<count; j++) {
										value = BitConverter.ToInt32(vertexBufferData, vertexOffset);
										if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 32);
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

					string semantic = element.GetProperty("semantic").GetString().Replace("_tfx_vb_semantic_", "");
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
					int semantic_index = (int) element.GetProperty("semantic_index").GetInt32();
					if (semantic_index != 0) semantic_index--;
					vertexBuffer[vertexIndex].Add(semantic+semantic_index, values.ToArray());
				}
				vertexIndex++;
			}
		}
		return vertexBuffer;
	}
	
	public static List<dynamic> parseTGXAsset(ExpandoObject staticTgxBin) 
	{
		Console.WriteLine("Parsing model data...");
		dynamic tgxBin = staticTgxBin;
		
		dynamic metadata = tgxBin.metadata; // Arrangement

		dynamic meshes = new List<dynamic>();

		for (var r=0; r<metadata.render_model.GetProperty("render_meshes").GetArrayLength(); r++) {
			Console.WriteLine("Parsing object "+r+"...");

			var renderMeshIndex = r;
			dynamic renderMesh = metadata.render_model.GetProperty("render_meshes")[renderMeshIndex]; // BoB Bunch of Bits


			// IndexBuffer
			Console.Write("Parsing object "+r+" index buffer... ");
			var indexBufferInfo = renderMesh.GetProperty("index_buffer");
			byte[] indexBufferData = tgxBin.files[indexBufferInfo.GetProperty("file_name").GetString()].data;

			dynamic indexBuffer = new List<ushort>();
			for (int j=0; j<indexBufferInfo.GetProperty("byte_size").GetInt32(); j+=indexBufferInfo.GetProperty("value_byte_size").GetInt32()) {
				var indexValue = BitConverter.ToUInt16(indexBufferData, j);
				indexBuffer.Add(indexValue);
			}
			//console.log('IndexBuffer', indexBufferInfo);
			Console.WriteLine("Done.");

			// VertexBuffer
			Console.Write("Parsing object "+r+" vertex buffers... ");
			List<Dictionary<string,dynamic>> vertexBuffer = parseVertexBuffers(staticTgxBin, renderMesh);
			Console.WriteLine("Done.");

			List<Dictionary<string,dynamic>> parts = new List<Dictionary<string,dynamic>>();
			List<int> partIndexList = new List<int>();
			int[] stagesToRender = {0, 7, 15}; // Hardcoded?
			//dynamic partOffsets = new JArray();

			Console.Write("Parsing object "+r+" stage parts... ");
			for(int sP=0; sP<renderMesh.GetProperty("stage_part_list").GetArrayLength(); sP++) {
				dynamic stagePart = renderMesh.GetProperty("stage_part_list")[sP];
				if (stagePart.ValueKind == JsonValueKind.Undefined) 
				{
				    //console.warn('MissingStagePart['+renderMeshIndex+':'+partOffset+']');
				    //Console.WriteLine("MissingStagePart["+renderMeshIndex+":"+partOffset+"]");
				    continue;
				}

				if (partIndexList.IndexOf(stagePart.GetProperty("start_index").GetInt32()) != -1) {
					// Skip duplicate parts
					continue;
				}
				partIndexList.Add(stagePart.GetProperty("start_index").GetInt32());
				parts.Add(parseStagePart(stagePart));
			}
			Console.WriteLine("Done.");

			// Spasm.RenderMesh
			dynamic mesh = new ExpandoObject();
				mesh.positionOffset = renderMesh.GetProperty("position_offset");
				mesh.positionScale = renderMesh.GetProperty("position_scale");
				mesh.texcoordOffset = renderMesh.GetProperty("texcoord_offset");
				mesh.texcoordScale = renderMesh.GetProperty("texcoord_scale");
				mesh.texcoord0ScaleOffset = renderMesh.GetProperty("texcoord0_scale_offset");
				mesh.indexBuffer = indexBuffer;
				mesh.vertexBuffer = vertexBuffer;
				mesh.parts = parts;
			
			meshes.Add(mesh);
		}

		Console.WriteLine("Model data parsed.");
		return meshes;
	}
}