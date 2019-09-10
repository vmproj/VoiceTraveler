
using System;

using System.Collections.Generic;

using System.Diagnostics;

using System.Numerics;

using System.Runtime.InteropServices;
using Accord.Compat;
using Accord.Math;
using Accord.Math.Transforms;



public static class PitchAccord
{
    private static int dB;
    public static int? GetDB()
    {
        return dB;
    }
    public static double? EstimateBasicFrequency(int sampleRate, float[] samples, int PowerAverageValue) //ReadOnlySpan<float>

    {

        // SDF

        // 1. zero pad

        var sdf = new Complex[samples.Length * 2];

        for (var i = 0; i < samples.Length; i++)

            sdf[i] = samples[i];



        // 2. FFT

        FourierTransform2.FFT(sdf, FourierTransform.Direction.Forward);



        // 3. パワースペクトル

        bool isPower = false;
        double total = 0f;
        for (var i = 0; i < sdf.Length; i++)

        {
            var x = sdf[i];

            sdf[i] = x.Real * x.Real + x.Imaginary * x.Imaginary;

            total += sdf[i].Magnitude;
        }

        //デシベル変換(パワー平均値を求める)
        dB = (int)(20 * Math.Log10(total / sampleRate));
        //UnityEngine.Assertions.Assert.IsTrue(dB > 100, dB.ToString() + " dB");
        if (dB > PowerAverageValue)
            isPower = true;

        //デシベルを下回ったら
        if (!isPower)
            return null;

        // 4. inverse FFT

        FourierTransform2.FFT(sdf, FourierTransform.Direction.Backward);



        // NSDF

        var nsdf = new double[samples.Length];

        var m = 0.0;

        for (var i = 0; i < samples.Length; i++)

        {

            var x = (double)samples[i];

            m += x * x;

            var inv = samples.Length - i - 1;

            x = samples[inv];

            m += x * x;



            nsdf[inv] = 2.0 * sdf[inv].Real / m;

        }



        // ピーク検出

        var maxCorrelation = 0.0;

        var peaks = new List<PeakPoint>();

        for (var i = 1; i < samples.Length; i++)

        {

            if (nsdf[i] > 0 && nsdf[i - 1] <= 0)

            {

                var currentMax = new PeakPoint(i, nsdf[i]);

                i++;



                for (; i < samples.Length; i++)

                {

                    if (nsdf[i] > currentMax.Correlation)

                    {

                        currentMax.Delay = i;

                        currentMax.Correlation = nsdf[i];

                    }

                    else if (nsdf[i] < 0)

                    {

                        // 0 未満になったので一山終了

                        break;

                    }

                }



                peaks.Add(currentMax);

                if (currentMax.Correlation > maxCorrelation)

                    maxCorrelation = currentMax.Correlation;

            }

        }



        if (peaks.Count == 0) return null; // 推定失敗



        var threshold = maxCorrelation * 0.8;

        var mainPeak = peaks.Find(x => x.Correlation >= threshold);



        var delay = mainPeak.Delay == nsdf.Length - 1

            ? mainPeak.Delay

            : GetTopX(

                mainPeak.Delay - 1, nsdf[mainPeak.Delay - 1],

                mainPeak.Delay, mainPeak.Correlation,

                mainPeak.Delay + 1, nsdf[mainPeak.Delay + 1]

            );



        Accord.Diagnostics.Debug.Assert(delay >= mainPeak.Delay - 1 && delay <= mainPeak.Delay + 1);

        return sampleRate / delay;

    }



    /// <summary>

    /// 2次関数として放物線補間して頂点のX座標を求める

    /// </summary>

    private static double GetTopX(double x1, double y1, double x2, double y2, double x3, double y3)

    {

        // y = ax^2 + bx + c



        var m = new[,]

        {

                { x1 * x1, x1, 1.0 },

                { x2 * x2, x2, 1.0 },

                { x3 * x3, x3, 1.0 }

            };



        // 行列で連立方程式を解くやつ

        var solution = m.Inverse(true).Dot(new[,] { { y1 }, { y2 }, { y3 } });



        var a = solution[0, 0];

        var b = solution[1, 0];

        //var c = solution[2, 0];



        // 頂点の公式

        return -b / (2.0 * a);

    }



    [StructLayout(LayoutKind.Auto)]

    private struct PeakPoint

    {

        public int Delay { get; set; }

        public double Correlation { get; set; }



        public PeakPoint(int delay, double correlation)

        {

            this.Delay = delay;

            this.Correlation = correlation;

        }

    }

}

