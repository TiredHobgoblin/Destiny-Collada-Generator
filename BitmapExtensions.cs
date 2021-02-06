using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
	public static class BitmapExtension
	{
		public static readonly ColorPalette GrayscalePalette = GenerateGrayscalePalette();

		private static ColorPalette GenerateGrayscalePalette()
		{
			using (Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
			{
				ColorPalette palette = bitmap.Palette;
				for (int i = 0; i < 256; i++)
				{
					palette.Entries[i] = Color.FromArgb(i, i, i);
				}
				return palette;
			}
		}

		public static void ColorPaletteToLookupTable(ColorPalette palette, out Matrix<byte> bTable, out Matrix<byte> gTable, out Matrix<byte> rTable, out Matrix<byte> aTable)
		{
			bTable = new Matrix<byte>(256, 1);
			gTable = new Matrix<byte>(256, 1);
			rTable = new Matrix<byte>(256, 1);
			aTable = new Matrix<byte>(256, 1);
			byte[,] data = bTable.Data;
			byte[,] data2 = gTable.Data;
			byte[,] data3 = rTable.Data;
			byte[,] data4 = aTable.Data;
			Color[] entries = palette.Entries;
			for (int i = 0; i < entries.Length; i++)
			{
				Color color = entries[i];
				data[i, 0] = color.B;
				data2[i, 0] = color.G;
				data3[i, 0] = color.R;
				data4[i, 0] = color.A;
			}
		}

		public static Bitmap RawDataToBitmap(IntPtr scan0, int step, Size size, Type srcColorType, int numberOfChannels, Type srcDepthType, bool tryDataSharing = false)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Invalid comparison between Unknown and I4
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Invalid comparison between Unknown and I4
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Expected O, but got Unknown
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			if (tryDataSharing)
			{
				if (srcColorType == typeof(Gray) && srcDepthType == typeof(byte))
				{
					return new Bitmap(size.Width, size.Height, step, PixelFormat.Format8bppIndexed, scan0)
					{
						Palette = GrayscalePalette
					};
				}
				if ((int)Platform.OperationSystem == 1 && (int)Platform.ClrType == 1 && srcColorType == typeof(Rgb) && srcDepthType == typeof(byte) && (step & 3) == 0)
				{
					return new Bitmap(size.Width, size.Height, step, PixelFormat.Format24bppRgb, scan0);
				}
				if (srcColorType == typeof(Rgba) && srcDepthType == typeof(byte))
				{
					return new Bitmap(size.Width, size.Height, step, PixelFormat.Format32bppArgb, scan0);
				}
			}
			PixelFormat pixelFormat;
			if (srcColorType == typeof(Gray))
			{
				pixelFormat = PixelFormat.Format8bppIndexed;
			}
			else if (srcColorType == typeof(Rgba))
			{
				pixelFormat = PixelFormat.Format32bppArgb;
			}
			else
			{
				if (!(srcColorType == typeof(Rgb)))
				{
					Mat val = (Mat)(object)new Mat(size.Height, size.Width, CvInvoke.GetDepthType(srcDepthType), numberOfChannels, scan0, step);
					try
					{
						Mat val2 = (Mat)(object)new Mat();
						try
						{
							CvInvoke.CvtColor((IInputArray)(object)val, (IOutputArray)(object)val2, srcColorType, typeof(Rgb));
							return RawDataToBitmap(val2.DataPointer, val2.Step, val2.Size, typeof(Rgb), 3, srcDepthType);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				pixelFormat = PixelFormat.Format24bppRgb;
			}
			Bitmap bitmap = new Bitmap(size.Width, size.Height, pixelFormat);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.WriteOnly, pixelFormat);
			Mat val3 = (Mat)(object)new Mat(size.Height, size.Width, (DepthType)0, numberOfChannels, bitmapData.Scan0, bitmapData.Stride);
			try
			{
				Mat val4 = (Mat)(object)new Mat(size.Height, size.Width, CvInvoke.GetDepthType(srcDepthType), numberOfChannels, scan0, step);
				try
				{
					if (srcDepthType == typeof(byte))
					{
						val4.CopyTo((IOutputArray)(object)val3, (IInputArray)null);
					}
					else
					{
						double num = 1.0;
						double num2 = 0.0;
						RangeF valueRange = val4.GetValueRange();
						if ((double)((RangeF)(valueRange)).Max > 255.0 || ((RangeF)(valueRange)).Min < 0f)
						{
							num = (((RangeF)(valueRange)).Max.Equals(((RangeF)(valueRange)).Min) ? 0.0 : (255.0 / (double)(((RangeF)(valueRange)).Max - ((RangeF)(valueRange)).Min)));
							num2 = (num.Equals(0.0) ? ((double)((RangeF)(valueRange)).Min) : ((double)(0f - ((RangeF)(valueRange)).Min) * num));
						}
						CvInvoke.ConvertScaleAbs((IInputArray)(object)val4, (IOutputArray)(object)val3, num, num2);
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			bitmap.UnlockBits(bitmapData);
			if (pixelFormat == PixelFormat.Format8bppIndexed)
			{
				bitmap.Palette = GrayscalePalette;
			}
			return bitmap;
		}

		public static Bitmap ToBitmap(this Mat mat)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			if (mat.Dims > 3)
			{
				return null;
			}
			int numberOfChannels = mat.NumberOfChannels;
			Size size = mat.Size;
			Type typeFromHandle;
			switch (numberOfChannels)
			{
				case 1:
					typeFromHandle = typeof(Gray);
					if (size.Equals(Size.Empty))
					{
						return null;
					}
					if ((size.Width | 3) != 0)
					{
						Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format8bppIndexed);
						bitmap.Palette = GrayscalePalette;
						BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
						Mat val = (Mat)(object)new Mat(size.Height, size.Width, (DepthType)0, 1, bitmapData.Scan0, bitmapData.Stride);
						try
						{
							mat.CopyTo((IOutputArray)(object)val, (IInputArray)null);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
						bitmap.UnlockBits(bitmapData);
						return bitmap;
					}
					break;
				case 3:
					typeFromHandle = typeof(Rgb);
					break;
				case 4:
					typeFromHandle = typeof(Rgba);
					break;
				default:
					throw new Exception("Unknown color type");
			}
			return RawDataToBitmap(mat.DataPointer, mat.Step, size, typeFromHandle, mat.NumberOfChannels, CvInvoke.GetDepthType(mat.Depth), tryDataSharing: true);
		}

		public static Bitmap ToBitmap(this UMat umat)
		{
			Mat mat = umat.GetMat((AccessType)16777216);
			try
			{
				return mat.ToBitmap();
			}
			finally
			{
				((IDisposable)mat)?.Dispose();
			}
		}

		public static Bitmap ToBitmap(this GpuMat gpuMat)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			Mat val = (Mat)(object)new Mat();
			try
			{
				gpuMat.Download((IOutputArray)(object)val, (Stream)null);
				return val.ToBitmap();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}