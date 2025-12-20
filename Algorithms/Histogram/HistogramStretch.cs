namespace Algorithms.Histogram;

public static class HistogramStretch
{
    public static byte[] CalculateStretch(int[] frequencies)
    {
        var min = 0;
        
        for (var i = 0; i < frequencies.Length; i++)
        {
            if (frequencies[i] != 0)
            {
                min = i;
                break;
            }
        }
        
        var max = 255;

        for (var i = frequencies.Length - 1; i >= 0; i--)
        {
            if (frequencies[i] != 0)
            {
                max = i;
                break;
            }
        }

        var recodingTable = new byte[256];

        for (var i = 0; i < 256; i++)
        {
            if (i < min) recodingTable[i] = 0;
            else if (i > max) recodingTable[i] = 255;
            else recodingTable[i] = (byte)((i-min) * 255 / (max-min));
        }
        
        return recodingTable;
    }

    public static byte[] CalculateStretchWithSaturation(int[] frequencies)
    {
        var totalPixels = frequencies.Sum();
        var ignoredPixels = totalPixels * 0.05; // number of pixels to ignore

        var min = 0;
        var ignoredMin = 0;
        
        for (var i = 0; i < frequencies.Length; i++)
        {
            if (frequencies[i] != 0)
            {
                ignoredMin += frequencies[i];
            }

            if (ignoredMin >= ignoredPixels)
            {
                min = i;
                break;
            }
        }

        var max = 255;
        var ignoredMax = 0;
        
        for (var i = frequencies.Length - 1; i >= 0; i--)
        {
            if (frequencies[i] != 0)
            {
                ignoredMax += frequencies[i];
            }
            
            if (ignoredMax >= ignoredPixels)
            {
                max = i;
                break;
            }
        }
        
        var recodingTable = new byte[256];

        for (var i = 0; i < 256; i++)
        {
            if (i < min) recodingTable[i] = 0;
            else if (i > max) recodingTable[i] = 255;
            else recodingTable[i] = (byte)((i-min) * 255 / (max-min));
        }

        return recodingTable;
    }

    public static byte[] Equalize(int[] frequencies)
    {
        var totalPixels = frequencies.Sum();
        var prefixSum = new int[frequencies.Length];
        var empiricDistribution = new double[256];
        
        prefixSum[0] = frequencies[0];
        empiricDistribution[0] = frequencies[0] / (double)totalPixels;
        
        for (var i = 1; i < frequencies.Length; i++)
        {
            prefixSum[i] = frequencies[i] + prefixSum[i - 1];
            empiricDistribution[i] = prefixSum[i] / (double)totalPixels;
        }

        var d0 = empiricDistribution.First(v => v > 0);
        
        var recodingTable = new byte[256];

        for (var i = 0; i < 256; i++)
        {
            recodingTable[i] = (byte)(((empiricDistribution[i] - d0) / (1 - d0)) * 255);
        }
        
        return recodingTable;
    }

    public static byte[] StretchByValues(int p1, int p2, int q1, int q2)
    {
        var lut = new byte[256];
    
        for (var i = 0; i < 256; i++)
        {
            if (i < p1)
                lut[i] = (byte)q1;
            else if (i > p2)
                lut[i] = (byte)q2;
            else
                lut[i] = (byte)(q1 + (i - p1) * (q2 - q1) / (p2 - p1));
        }
    
        return lut;
    }
}