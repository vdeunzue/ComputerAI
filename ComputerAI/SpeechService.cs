using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

namespace ComputerAI
{
    public static class SpeechService
    {
        public static async Task TextToSpeechAsync(string text)
        {
            var config = SpeechConfig.FromSubscription(Constants.SubscriptionKey, Constants.ServiceRegion);

            using var synthesizer = new SpeechSynthesizer(config);
            var ssml = $@"
                    <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
                        <voice name='{Constants.VoiceName}'>
                            <prosody pitch='11%' contour='(11%, +15%) (60%, -23%) (80%, -34%)' rate='+35%'>
                                {text}
                            </prosody>
                        </voice>
                    </speak>";

            await synthesizer.SpeakSsmlAsync(ssml);
        }

        public static async Task<string> SpeechToTextAsync()
        {
            var speechConfig = SpeechConfig.FromSubscription(Constants.SubscriptionKey, Constants.ServiceRegion);
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    return result.Text;
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine("ERROR: No speech could be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    }
                }
                else
                {
                    Console.WriteLine($"UNKNOWN ERROR");
                }

                return string.Empty;
            }
        }
    }
}
