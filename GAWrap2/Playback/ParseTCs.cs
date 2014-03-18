using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GAWrap2.Playback
{
    class ParseTCs
    {
        // Put csv instructions file into data object
        public List<StepLite> readTCs(string file, int length)
        {
            List<StepLite> sList = new List<StepLite>();

            //Read events from file and stores them in a list of eventStore objects:
            using (StreamReader instructs = new StreamReader(file))
            {
                string line;
                int index = 0;
                while ((line = instructs.ReadLine()) != null)
                {
                    if (line.Split(',')[0] == "image")
                    {
                        if (index != -1)
                            if (++index > length)
                                break;

                        sList.Add(new StepLite(line.Split(',')[1]));
                    }
                    else if (sList.Count > 0)
                        sList[sList.Count - 1].events.Add(line);
                }
            }

            delIgnored(sList);
            cleanDoubleClick(sList);

            return sList;
        }


        //Remove ignored steps:
        private void delIgnored(List<StepLite> sList)
        {
            for (int i = Playback.data.cfg.steps.Count - 1; i >= 0; i--)
                sList.RemoveAt(Playback.data.cfg.steps[i]);

            //Remove time stamps:
            for (int i = 0; i < sList.Count; i++)
                if (sList[i].events[0].Split(',').Length == 1)
                    sList[i].events.RemoveAt(0);
        }


        //Double click is also recorded as two click down and click up events.
        //Need to remove the unnecessary events
        private void cleanDoubleClick(List<StepLite> sList)
        {
            List<string> sE;

            Func<List<StepLite>, int, int> remStep = (s, i) =>
            {
                s[i].setImage(s[i - 1].image);
                s.RemoveAt(i - 1);

                return (i - 1);
            };

            //Find doubleclick instances and delete the step before them:
            for (int i = 0; i < sList.Count; i++)
            {
                sE = sList[i].events;

                for (int j = 0; j < sE.Count; j++)
                {
                    if (sE[j].Split(',')[0].Contains("doubleclick"))
                    {
                        sE.RemoveAt(j + 1);
                        sE.RemoveAt(j - 1);

                        if (i > 0)
                            i = remStep(sList, i);

                        break;
                    }
                }
            }
        }
    }
}
