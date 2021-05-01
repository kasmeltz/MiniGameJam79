namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;

    public class Dialogue 
    {
        #region Constructor
        public Dialogue(params string[] speakers)
        {
            SpeakerKeys = speakers
                .ToList();

            Lines = new List<DialogueLine>();
        }

        #endregion

        #region Members

        public List<string> SpeakerKeys { get; set; }
        
        public List<DialogueLine> Lines { get; set; }

        #endregion

        #region Public Methods

        public void AddLine(int speakerIndex, string text)
        {
            var line = new DialogueLine { SpeakerIndex = speakerIndex, Text = text };

            Lines
                .Add(line);
        }

        #endregion
    }
}