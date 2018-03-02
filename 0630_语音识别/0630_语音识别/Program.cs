using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Kinect;

namespace _0630_语音识别
{
    class Program
    {
    static void Main(string[] args)
        {
            
            KinectSensor sensor =
            foreach (var potentialSensor in sensor.KinectSensors) //所有支持foreach的集合类都继承IEnumerable接口  
            {  
               if (potentialSensor.Status == KinectStatus.Connected)  
               {  
                this.sensor = potentialSensor;  
                break;  
               }  
            }  
            if (sensor == null)
            {
                Console.WriteLine("No Kinect Connected\n" + "Press any key to continue.\n");
                Console.ReadKey(true);
                return;
            }
            sensor.Start();
        AudioSource audioSource = sensor.AudioSource;
        audioSource.EchoCancellationMode = EchoCancellationMode.None;
        audioSource.AutomaticGainControlEnabled = false;
        RecognizerInfo recognizerInfo = GetKinectRecognizer();
        using (var speechRecognitionEngine = new SpeechRecognitionEngine(recognizerInfo.Id))
        {
            var colors = new Choices();
            colors.Add("help");
            colors.Add("green");
            colors.Add("blue");
            var grammatBuilder = new GrammarBuilder { Culture = recognizerInfo.Culture };
            grammatBuilder.Append(colors);
            var g = new Grammar(grammatBuilder);
            speechRecognitionEngine.LoadGrammar(g);
            speechRecognitionEngine.SpeechRecognized += SreSpeechRecognized;
            speechRecognitionEngine.SpeechHypothesized += SreSpeechHypothesized;
            speechRecognitionEngine.SpeechRecognitionRejected += SreSpeechRecognitionRejected;
            using (Stream s = audioSource.Start())
            {
                speechRecognitionEngine.SetInputToAudioStream(
                    s, new SpeechAudioFormatInfo(EncodingFormat.Pcm,
                    16000, 16, 1, 32000, 2, null));
                Console.WriteLine(
                    "Recognizing speech. Say: 'help', 'green' or 'blue'. Press ENTER to stop");
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                Console.ReadLine();
                Console.WriteLine("Stopping recognizer ...");
                speechRecognitionEngine.RecognizeAsyncStop();
            }
        }
    }
    private static RecognizerInfo GetKinectRecognizer()
    {
        Func<RecognizerInfo, bool> matchingFunc = r =>
        {
            string value;
            r.AdditionalInfo.TryGetValue("Kinect", out value);
            return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) &&
                "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
        };
        return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).
               FirstOrDefault();
    }
    private static void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
    {
        Console.WriteLine("\nSpeech Rejected");
        if (e.Result != null)
        {
            DumpRecordedAudio(e.Result.Audio);
        }
    }
    private static void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
    {
        Console.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
    }
    private static void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        if (e.Result.Confidence >= 0.7)
        {
            Console.WriteLine("\nSpeech Recognized: \t{0}\tConfidence:\t{1}",
                e.Result.Text, e.Result.Confidence);
        }
        else
        {
            Console.WriteLine("\nSpeech Recognized but confidence was too low: \t{0}",
                e.Result.Confidence);
            DumpRecordedAudio(e.Result.Audio);
        }
    }
    private static void DumpRecordedAudio(RecognizedAudio audio)
    {
        if (audio == null) return;

        int fileId = 0;
        string filename;
        while (File.Exists((filename = "RetainedAudio_" + fileId + ".wav")))
            fileId++;

        Console.WriteLine("\nWriting file: {0}", filename);
        using (var file = new FileStream(filename, System.IO.FileMode.CreateNew))
            audio.WriteToWaveStream(file);
    }
    }
}
