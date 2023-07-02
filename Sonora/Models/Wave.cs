namespace Sonora.Models;

public class Wave
{
    public float[] Data { get; }
    public uint Channels { get; }
    public ulong Frames { get; }
    public ulong Samples => Channels * Frames;

    public float this[ulong index] => Data[index];

    public Wave()
    {
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
        if (!HostInterface.Instance.IsValid)
        {
            Data = new float[] { 0 };
            return;
        }

        var wave = HostInterface.Instance.WaveNewFromFile(filename);
        if (wave == 0)
        {
            Data = new float[] { 0 };
            return;
        }

        try
        {
            Channels = HostInterface.Instance.WaveGetChannels(wave);
            Frames = HostInterface.Instance.WaveGetFrames(wave);
            Data = new float[Samples];
            HostInterface.Instance.WaveReadFrames(wave, Data, (ulong)Data.LongLength);
        }
        finally
        {
            HostInterface.Instance.WaveFree(wave);
        }
    }
}