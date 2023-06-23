using System;
using System.Runtime.InteropServices;
using CSCore.Codecs.WAV;
using OpenTK.Audio.OpenAL;

namespace CSharp_PG2;

public class Audio : IDisposable
{
    private ALDevice _device;
    private ALContext _context;
    private int _source;
    private int _buffer;
    public ALContext Context => _context;

    public Audio()
    {
        
        _device = ALC.OpenDevice(null);
        if (_device != null)
        {
            _context = ALC.CreateContext(_device, (int[])null);
            ALC.MakeContextCurrent(_context);
            ALError error = AL.GetError();
            if (error != ALError.NoError)
            {
                Console.WriteLine("Failed to create OpenAL context");
            }

            _buffer = AL.GenBuffer();
            _source = AL.GenSource();
        }
        else
        {
            Console.WriteLine("Failed to open the audio device.");
        }
    }

    public void Load(string path)
    {
        LoadWave(path, _buffer);
        AL.Source(_source, ALSourcei.Buffer, _buffer);
        AL.Source(_source, ALSourceb.Looping, true);
        AL.Source(_source, ALSourcef.Gain, 0.2f);
        AL.Source(_source, ALSourcef.Pitch, 1.0f);
     
    }

    public void Play()
    {
        AL.SourcePlay(_source);
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            Console.WriteLine($"OpenAL error: {error}");
        }
    }

    public void Pause()
    {
        AL.SourcePause(_source);
    }

    public void Resume()
    {
        AL.SourcePlay(_source);
    }

    public void Stop()
    {
        AL.SourceStop(_source);
    }

    private void LoadWave(string path, int buffer)
    {
        using (var reader = new WaveFileReader(path))
        {
            var waveFormat = reader.WaveFormat;
            var audioData = new byte[reader.Length];
            reader.Read(audioData, 0, audioData.Length);

            var format = (waveFormat.Channels == 1) ? ((waveFormat.BitsPerSample == 8) ? ALFormat.Mono8 : ALFormat.Mono16) :
                ((waveFormat.BitsPerSample == 8) ? ALFormat.Stereo8 : ALFormat.Stereo16);

            IntPtr audioDataPtr = Marshal.AllocHGlobal(audioData.Length);
            Marshal.Copy(audioData, 0, audioDataPtr, audioData.Length);

            AL.BufferData(buffer, format, audioDataPtr, audioData.Length, waveFormat.SampleRate);

            Marshal.FreeHGlobal(audioDataPtr);
        }
    }

    public void Dispose()
    {
        Stop();
        AL.DeleteSource(_source);
        AL.DeleteBuffer(_buffer);

        ALC.MakeContextCurrent(default);
        ALC.DestroyContext(_context);
        ALC.CloseDevice(_device);
    }
}