// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Extensions of <see cref = "IAdjustment" />.
    /// </summary>
    public static class AdjustmentExtensions
    {

        /// <summary>
        /// Returns the matrix 
        /// ( 
        ///    1, 0, 0, 0,
        ///    0, 1, 0, 0,
        ///    0, 0, 1, 0,
        ///    0, 0, 0, 0,
        /// ).
        /// </summary>
        public static readonly Matrix5x4 One = new Matrix5x4
        {
#pragma warning disable IDE0055
            M11 = 1, M12 = 0, M13 = 0, M14 = 0, // Red
            M21 = 0, M22 = 1, M23 = 0, M24 = 0, // Green
            M31 = 0, M32 = 0, M33 = 1, M34 = 0, // Blue
            M41 = 0, M42 = 0, M43 = 0, M44 = 1, // Alpha
            M51 = 0, M52 = 0, M53 = 0, M54 = 0 // Offset
#pragma warning restore IDE0055
        };


        /// <summary>
        /// Turn into color hdr.
        /// </summary>
        /// <param name="color"> The source color. </param>
        /// <returns> The product color hdr. </returns>
        public static Vector4 ToColorHdr(this Color color) => new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        /// <summary>
        /// Turn into color.
        /// </summary>
        /// <param name="colorHdr"> The source color hdr. </param>
        /// <returns> The product color. </returns>
        public static Color ToColor(this Vector4 colorHdr) => Color.FromArgb((byte)(colorHdr.W * 255f), (byte)(colorHdr.X * 255f), (byte)(colorHdr.Y * 255f), (byte)(colorHdr.Z * 255f));


        /// <summary>
        /// Gets color matching.
        /// </summary>
        /// <param name="source"> The source color. </param>
        /// <param name="destination"> The destination color. </param>
        /// <returns> The product matrix. </returns>
        public static Matrix5x4 GetColorMatching(Color source, Color destination) => AdjustmentExtensions.GetColorMatching(new Vector4(source.R / 255f, source.G / 255f, source.B / 255f, source.A / 255f), new Vector4(destination.R / 255f, destination.G / 255f, destination.B / 255f, destination.A / 255f));
        /// <summary>
        /// Gets color matching.
        /// </summary>
        /// <param name="sourceHdr"> The source color hdr. </param>
        /// <param name="destinationHdr"> The destination color hdr. </param>
        /// <returns> The product matrix. </returns>
        public static Matrix5x4 GetColorMatching(Vector4 sourceHdr, Vector4 destinationHdr) => new Matrix5x4
        {
#pragma warning disable IDE0055
            M11 = sourceHdr.X, M12 = 0, M13 = 0, M14 = 0, // Red
            M21 = 0, M22 = sourceHdr.Y, M23 = 0, M24 = 0, // Green
            M31 = 0, M32 = 0, M33 = sourceHdr.Z, M34 = 0, // Blue
            M41 = 0, M42 = 0, M43 = 0, M44 = sourceHdr.W, // Alpha
            M51 = destinationHdr.X, M52 = destinationHdr.Y, M53 = destinationHdr.Z, M54 = destinationHdr.W // Offset
#pragma warning restore IDE0055
        };


        /// <summary>
        /// Gets source color.
        /// </summary>
        /// <param name="colorMatrix"> The color mtrix. </param>
        /// <returns> The product color. </returns>
        public static Color GetSourceColor(this Matrix5x4 colorMatrix) => AdjustmentExtensions.ToColor(AdjustmentExtensions.GetSourceColorHdr(colorMatrix));
        /// <summary>
        /// Gets source color hdr.
        /// </summary>
        /// <param name="colorMatrix"> The color mtrix. </param>
        /// <returns> The product color hdr. </returns>
        public static Vector4 GetSourceColorHdr(this Matrix5x4 colorMatrix) => new Vector4(colorMatrix.M11, colorMatrix.M22, colorMatrix.M33, colorMatrix.M44);

        /// <summary>
        /// Gets destination color.
        /// </summary>
        /// <param name="colorMatrix"> The color mtrix. </param>
        /// <returns> The product color. </returns>
        public static Color GetDestinationColor(this Matrix5x4 colorMatrix) => AdjustmentExtensions.ToColor(AdjustmentExtensions.GetDestinationColorHdr(colorMatrix));
        /// <summary>
        /// Gets destination color hdr.
        /// </summary>
        /// <param name="colorMatrix"> The color mtrix. </param>
        /// <returns> The product color hdr. </returns>
        public static Vector4 GetDestinationColorHdr(this Matrix5x4 colorMatrix) => new Vector4(colorMatrix.M51, colorMatrix.M52, colorMatrix.M53, colorMatrix.M54);

    }
}