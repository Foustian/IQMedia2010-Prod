using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sentiment.HelperClasses;
using Sentiment.Domain.Factory;
using Sentiment.Domain.Interface;
using System.Xml;
using System.Text.RegularExpressions;

namespace Sentiment.Logic
{
    public class SentimentLogic
    {
        private List<IQ_Sentiment> _ListOfIQ_Sentiment;

        public Dictionary<string, Sentiments> GetSentiment(Dictionary<string, List<string>> p_MapIQCCKeyToListOfHighlight, float p_LowThreshold, float p_HighThreshold, Guid p_ClientGuid)
        {
            Dictionary<string, Sentiments> _ListOfSentiment = new Dictionary<string, Sentiments>();
            try
            {
                if (_ListOfIQ_Sentiment == null)
                {
                    GetIQ_Sentiments(p_ClientGuid);
                }

                foreach (string Key in p_MapIQCCKeyToListOfHighlight.Keys)
                {
                    List<string> _ListOfHighlights;
                    List<SubSentiment> _MapHighlightToWeight = new List<SubSentiment>();
                    Sentiments _Sentiment = new Sentiments(); 
                    if(p_MapIQCCKeyToListOfHighlight.TryGetValue(Key,out _ListOfHighlights))
                    {
                        foreach (string _HighlightText in _ListOfHighlights)
                        {
                            // remove ns: and <span> tag from fragment
                            string _Text = Regex.Replace(_HighlightText.ToLower(), "(\\d*)(s:)", string.Empty).Replace("<span class=\"highlight\">", string.Empty).Replace("</span>", string.Empty);

                            // replace all special characters expect ' (signle quote)  to space'
                            _Text = Regex.Replace(_Text, "[^0-9a-zA-Z']+", " ");

                            // replace multiple spaces to a signle space.
                            _Text = Regex.Replace(_Text, @"\s{2,}", " ");

                            // get the list of words by splitting with <space> , and remove empty words if any. 
                            List<string> _ListOfWords = _Text.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                            //List<IQ_Sentiment> _ListOfIQ_SentimentSelected = _ListOfIQ_Sentiment.Where(s => _ListOfWords.Contains(s.Word.ToLower())).Select(s => s).ToList();

                            List<IQ_Sentiment> _ListOfIQ_SentimentSelected = _ListOfIQ_Sentiment.Join(_ListOfWords, q => q.Word.ToLower(), a => a.ToLower(),
                                                    (q, a) => q).ToList();



                            double _Weight = _ListOfIQ_SentimentSelected.Count > 0 ? (double)Math.Round(Convert.ToDouble(_ListOfIQ_SentimentSelected.Sum(s => s.Value)) / _ListOfWords.Count(), 5) : 0;

                            //CommonFunction.LogInfo("IQ_CC_KEY : " + hit.Iqcckey + " Fragment Weight => " + _Weight.ToString() + "", res.OriginalRequest.IsPmgLogging, res.OriginalRequest.PmgLogFileLocation);

                            _MapHighlightToWeight.Add(new SubSentiment() { HighlightingText = _HighlightText, Weight = _Weight });
                        }
                        _Sentiment.HighlightToWeightMap = _MapHighlightToWeight;
                        _Sentiment.PositiveSentiment = _MapHighlightToWeight.Where(a => a.Weight >= p_HighThreshold).Count();
                        _Sentiment.NegativeSentiment = _MapHighlightToWeight.Where(a => a.Weight <= p_LowThreshold).Count();
                        //_Sentiment.FullSentiment = _MapHighlightToWeight.Count();

                        _ListOfSentiment.Add(Key,_Sentiment);
                    
                    }    
                }
                return _ListOfSentiment;
            }
            catch (Exception ex)
            {
                return _ListOfSentiment;
            }

        }

        private void GetIQ_Sentiments(Guid p_ClientGuid)
        {
            
            ModelFactory _ModelFactory = new ModelFactory();
            IIQ_SentimentsModel _IIQ_SentimentsModel = _ModelFactory.CreateObject<IIQ_SentimentsModel>();
            _ListOfIQ_Sentiment = _IIQ_SentimentsModel.GetSentimentsByClientGuid(p_ClientGuid);
        }
    }
}
