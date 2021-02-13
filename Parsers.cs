using System;
using System.Text.Json;
using System.Dynamic;
using System.Collections.Generic;

namespace DestinyColladaGenerator
{
	// Methods used to parse large sections of the input
	class Parsers
	{
		public static SkinBufferData parseSkinBuffer (
			dynamic VertexBufferInfo,
			ExpandoObject TGXFileEntry = null
		)
		{
			dynamic info = VertexBufferInfo;
			dynamic file = TGXFileEntry;
			//debug("TGXLoader:SkinBuffer", VertexBufferInfo);

			SkinBufferData skinBuffer = new SkinBufferData
			(
				info.GetProperty("file_name").GetString(), //id
				new List<byte>(), //header
				new Dictionary<int, SkinBufferChunk>() //data
			);

			if (TGXFileEntry != null) {
				Boolean isHeader = true;
				int chunkWeight = 0;
				int weightOffset = 0;
				for (int i=0; i<info.GetProperty("byte_size").GetInt32(); i+=info.GetProperty("stride_byte_size").GetInt32()) {
				uint skinVertex = BitConverter.ToUInt32(file.data, i);

				if (info.GetProperty("stride_byte_size").GetInt32() != 4) {
					ConsoleEx.Warn("Skinbuffer stride is not equal to 4.");
				}

				byte index0 = (byte)((skinVertex >> 0) & 0xff);
				byte index1 = (byte)((skinVertex >> 8) & 0xff);
				byte weight0 = (byte)((skinVertex >> 16) & 0xff);
				byte weight1 = (byte)((skinVertex >> 24) & 0xff);

				if (chunkWeight == 0) {
					weightOffset = i;
				}
				int chunkIndex = weightOffset / 4;

				int strideIndex = i / 4;
				if (!skinBuffer.data.ContainsKey(strideIndex)) {
					skinBuffer.data.Add(strideIndex, new SkinBufferChunk(
						strideIndex,
						0,
						new List<byte>(),
						new List<byte>()
						)
					);
				}
				SkinBufferChunk chunkData = skinBuffer.data[chunkIndex];

				if (index0 != weight0) 
				{
					isHeader = false;
				}
				if (isHeader) 
				{
					skinBuffer.header.Add(index0);
					skinBuffer.header.Add(index1);
					skinBuffer.header.Add(weight0);
					skinBuffer.header.Add(weight1);
				} else if (skinVertex == 0) {
					//
				} else {
					//{weight0, weight1}.forEach((weight) => {
					foreach(byte weight in new byte[]{weight0, weight1})
					{
						if (weight > 0) 
						{
							chunkData.count++;
						}
						chunkWeight += weight;
					};

					chunkData.indices.Add(index0);
					chunkData.indices.Add(index1);
					chunkData.weights.Add(weight0);
					chunkData.weights.Add(weight1);
					if (chunkWeight >= 255) {
					chunkWeight = 0;
					}
				}
				}
			}
		return skinBuffer;
		}
		
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
						if (hasStaticTextures) value.staticTextures = staticTextures;
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
				ConsoleEx.Warn("Multiple Stage Part Vertex Layout Definitions", renderMesh.GetProperty("stage_part_vertex_stream_layout_definitions"));
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
						List<byte> rawBytes = new List<byte>();

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
											rawBytes.Add(vertexBufferData[vertexOffset]);
											vertexOffset++;
										}
										break;
									case "byte":
										for (j=0; j<count; j++) {
											value = TGXMUtils.Sbyte(vertexBufferData, vertexOffset);
											if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 8);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											vertexOffset++;
										}
										break;
									case "ushort":
										for(j=0; j<count; j++) {
											value = BitConverter.ToUInt16(vertexBufferData, vertexOffset);
											if (element.GetProperty("normalized").GetBoolean) value = TGXMUtils.unormalize(value, 16);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											rawBytes.Add(vertexBufferData[vertexOffset+1]);
											vertexOffset += 2;
										}
										break;
									case "short":
										for(j=0; j<count; j++) {
											value = BitConverter.ToInt16(vertexBufferData, vertexOffset);
											if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 16);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											rawBytes.Add(vertexBufferData[vertexOffset+1]);
											vertexOffset += 2;
										}
										break;
									case "uint":
										for(j=0; j<count; j++) {
											value = BitConverter.ToUInt32(vertexBufferData, vertexOffset);
											if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.unormalize(value, 32);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											rawBytes.Add(vertexBufferData[vertexOffset+1]);
											rawBytes.Add(vertexBufferData[vertexOffset+2]);
											rawBytes.Add(vertexBufferData[vertexOffset+3]);
											vertexOffset += 4;
										}
										break;
									case "int":
										for(j=0; j<count; j++) {
											value = BitConverter.ToInt32(vertexBufferData, vertexOffset);
											if (element.GetProperty("normalized").GetBoolean()) value = TGXMUtils.normalize(value, 32);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											rawBytes.Add(vertexBufferData[vertexOffset+1]);
											rawBytes.Add(vertexBufferData[vertexOffset+2]);
											rawBytes.Add(vertexBufferData[vertexOffset+3]);
											vertexOffset += 4;
										}
										break;
									case "float":
										for(j=0; j<count; j++) {
											value = BitConverter.ToSingle(vertexBufferData, vertexOffset);
											values.Add(value);
											rawBytes.Add(vertexBufferData[vertexOffset]);
											rawBytes.Add(vertexBufferData[vertexOffset+1]);
											rawBytes.Add(vertexBufferData[vertexOffset+2]);
											rawBytes.Add(vertexBufferData[vertexOffset+3]);
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
						vertexBuffer[vertexIndex].Add(semantic+semantic_index+"_raw", rawBytes.ToArray());
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

				// SkinBuffer
				SkinBufferData skinBuffer = new SkinBufferData("", new List<byte>(), new Dictionary<int, SkinBufferChunk>());
				var skinBufferInfo = new JsonElement();
				if (renderMesh.TryGetProperty("single_pass_skin_vertex_buffer", out skinBufferInfo)&&skinBufferInfo.GetProperty("byte_size").GetInt32()>0)
				{
					Console.Write("Parsing object "+r+" skinning buffer... ");
					skinBuffer = parseSkinBuffer(skinBufferInfo, tgxBin.files[skinBufferInfo.GetProperty("file_name").GetString()]);
					Console.WriteLine("Done.");
				}

				// VertexBuffer
				Console.Write("Parsing object "+r+" vertex buffers... ");
				List<Dictionary<string,dynamic>> vertexBuffer = parseVertexBuffers(staticTgxBin, renderMesh);
				Console.WriteLine("Done.");

				// DataDrivenVertexBuffer
				List<byte[]> dataDrivenVertexBuffer = new List<byte[]>();
				var dataDrivenVertexBufferInfo = new JsonElement();
				if (renderMesh.TryGetProperty("data_driven_vertex_buffer", out dataDrivenVertexBufferInfo)&&dataDrivenVertexBufferInfo.GetProperty("byte_size").GetInt32()>0)
				{
					Console.Write("Parsing object "+r+" data driven vertex buffer... ");
					byte[] rawBuffer = tgxBin.files[dataDrivenVertexBufferInfo.GetProperty("file_name").GetString()].data;
					for (int b=0; b<renderMesh.GetProperty("vertex_buffers")[0].GetProperty("byte_size").GetInt32()/32; b++)
					{
						//int startIndex = b*4;
						double[] vertBuff = new double[4]{0,0,0,0};//{rawBuffer[b], rawBuffer[b+1], rawBuffer[b+2], rawBuffer[b+3]};
						//dataDrivenVertexBuffer.Add(vertBuff);
						for (int c=0; c<4; c++)
						{
							if (b >= dataDrivenVertexBufferInfo.GetProperty("byte_size").GetInt32()/4)
								continue;
							vertBuff[c] = rawBuffer[(b*4)+c]/255.0;
						}
						vertexBuffer[b].Add("color0", vertBuff);
					}
					Console.WriteLine("Done.");
				}

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
					mesh.skinBuffer = skinBuffer;
					//mesh.dataDrivenVertexBuffer = dataDrivenVertexBuffer.ToArray();
					mesh.parts = parts;
				
				meshes.Add(mesh);
			}

			Console.WriteLine("Model data parsed.");
			return meshes;
		}
	}
}