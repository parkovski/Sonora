using System.IO;

namespace Sonora.Models;

public partial class Wave {
    public float[] Data { get; }
    public uint Channels { get; }
    public ulong Frames { get; }
    public ulong Samples => Channels * Frames;

    public float this[ulong index] => Data[index];

    public Wave() {
        Data = new float[] { 0 };
        Channels = 1;
        Frames = 1;
    }

    public Wave(float[] data, uint channels)
    {
        Data = data;
        Channels = channels;
        if (data.LongLength % channels != 0)
        {
            throw new ArgumentException("Expected samples to be a multiple of channels", nameof(data));
        }
        Frames = (ulong)data.LongLength / channels;
    }

    public Wave(string filename)
    {
        var wave = Native.SnrWaveNewFromFile(filename);
        if (wave == 0)
        {
            throw new FileNotFoundException(filename);
            //Data = new float[] { 0 };
            //return;
        }

        try
        {
            Channels = Native.SnrWaveGetChannels(wave);
            Frames = Native.SnrWaveGetFrames(wave);
            Data = new float[Samples];
            Native.SnrWaveReadFrames(wave, Data, (ulong)Data.LongLength);
        }
        finally
        {
            Native.SnrWaveFree(wave);
        }
    }
}
