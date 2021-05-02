namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("KasJam/DialoguePlayer")]
    public class DialoguePlayerBehaviour : BehaviourBase
    {
        #region Members

        public float LetterSpeed;
      
        public TMP_Text DialogueText;

        public Image SpeakerBubble;

        public Button AdvanceTextButton;

        public Image ShopPanel;
        
        public LevelManagerBehaviour LevelManager;

        public int DialogueIndex { get; set; } 

        protected List<Dialogue> Dialogues { get; set; }

        protected Dialogue Dialogue { get; set; }

        protected DialogueLine DialogueLine { get; set; }

        protected int DialogueLineIndex { get; set; }

        protected string LineToType { get; set; }

        protected int LineTypeIndex { get; set; }

        protected float LetterCountdown { get; set; }

        protected PlayMusicLooper PlayLooper { get; set; }

        protected MenuMusicLooper MenuLooper { get; set; }

        protected float VoiceTimer { get; set; }


        #endregion

        #region Events

        public event EventHandler DialogueCompleted;

        protected void OnDialogueCompleted()
        {
            DialogueCompleted?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void PlaySpeakerVoice()
        {
            VoiceTimer -= Time.deltaTime;
            if (VoiceTimer <= 0)
            {
                VoiceTimer = 0;

                VoiceTimer = UnityEngine
                    .Random
                    .Range(0.2f, 0.45f);

                if (DialogueLine != null)
                {
                    if (DialogueLine.SpeakerIndex == 0)
                    {
                        SoundEffects
                            .Instance
                            .DialogueKiki();
                    } else
                    {
                        SoundEffects
                            .Instance
                            .DialogueWiz();
                    }
                }
            }
        }

        public void ShowDialogue()
        {
            DialogueLine = Dialogue.Lines[DialogueLineIndex];

            LineToType = DialogueLine.Text;
            LineTypeIndex = 0;
            LetterCountdown = LetterSpeed;

            var scale = new Vector3(1, 1, 1);
            if (DialogueLine.SpeakerIndex == 0)
            {
                scale.x = -1;
            }

            SpeakerBubble.transform.localScale = scale;
        }

        public void StartDialogue()
        {
            if (DialogueIndex >= Dialogues.Count)
            {
                DialogueIndex--;
            }

            MenuLooper
                .EnsurePlaying(1f);

            MenuLooper
                .MoveToLoop(1);

            Dialogue = Dialogues[DialogueIndex];

            DialogueLineIndex = 0;

            AdvanceTextButton
                .gameObject
                .SetActive(true);

            ShowDialogue();
        }

        public void AdvanceDialogueText()
        {
            if (Dialogue == null)
            {
                return;
            }

            if (DialogueLineIndex >= Dialogue.Lines.Count)
            {
                return;
            }

            DialogueLineIndex++;

            if (DialogueLineIndex == Dialogue.OpenShopLine)
            {
                ShopPanel
                    .gameObject
                    .SetActive(true);
            }
            else
            {
                if (DialogueLineIndex >= Dialogue.Lines.Count)
                {
                    AdvanceTextButton
                        .gameObject
                        .SetActive(false);

                    DialogueIndex++;

                    CompleteDialogue();

                    return;
                }

                ShowDialogue();
            }
        }

        #endregion

        #region Protected Methods

        protected void CompleteDialogue()
        {
            OnDialogueCompleted();

            LevelManager
                .Reset();

            MenuLooper
                .EnsureNotPlaying();

            PlayLooper
                .EnsurePlaying(1.0f);

            gameObject
                .SetActive(false);
        }

        protected void CreateDialogues()
        {
            Dialogues = new List<Dialogue>();

            var dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.AddLine(0, "Dude- have you been outside lately?! Everything's gone bananas!");
            dialogue.AddLine(1, "Indeed! A terrible Weirdness has consumed Lilytopia!");
            dialogue.AddLine(0, "A 'Weirdness' you say? I guess that's... a bad thing?");
            dialogue.AddLine(1, "A VERY bad thing! To use a technical term... everyone has totally flipped!");
            dialogue.AddLine(0, "Everyone... except us?");
            dialogue.AddLine(1, "Looks like it.");
            dialogue.AddLine(0, "So we're screwed.");
            dialogue.AddLine(1, "Not quite. I have a plan. I am a Great and Powerful Frog Wizard, after all.");
            dialogue.AddLine(0, "Oh cool!");
            dialogue.AddLine(1, "I shall concoct a great Potion of Un-Weirdness to restore Lilytopia to its former glory!");
            dialogue.AddLine(0, "Aww, yes!");
            dialogue.AddLine(1, "But I need your help.");
            dialogue.AddLine(0, "Aww, no!");
            dialogue.AddLine(1, "First I must study the Weirdness to determine the exact nature of its mysterious properties. To do this, we'll need flies. LOTS of flies. And I'll need YOU to get them for me.");
            dialogue.AddLine(0, "But... I only just got here! Plus I'm only five minutes old!");
            dialogue.AddLine(1, "Yet it seems the Frogs of Fate have chosen you.");
            dialogue.AddLine(0, "Whoa! I love the Frogs of Fate!");
            dialogue.AddLine(0, "But isn't this, like, reeeeeally dangerous?");
            dialogue.AddLine(1, "Take this. My most precious relic - THE AMULET OF RESPAWNING! Whenever you die, you will instantly respawn right here.");
            dialogue.AddLine(0, "Whenever I WHAT?!");
            dialogue.AddLine(1, "I'm sure it's not as bad as it sounds. Now go! And good luck, little froglet!");
            dialogue.AddLine(0, "The name's Kiki.");
            dialogue.AddLine(1, "Whatever! Goodbye!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 8;
            dialogue.AddLine(0, "That. Was. GROSS! Dying is the absolute WORST!");
            dialogue.AddLine(1, "So I've heard. But we're one step closer to an antidote to this Weirdness.");
            dialogue.AddLine(0, "Well, okay then...");
            dialogue.AddLine(1, "There is something I should tell you. This is not my home- it's a shop!");
            dialogue.AddLine(0, "Capitalism?! At a time like this?");
            dialogue.AddLine(1, "The flies you bring me can be used to craft magical items and potions that will help you survive the Weirdness.");
            dialogue.AddLine(0, "Oh, actually yes, I would like some of those. ");
            dialogue.AddLine(1, "Here's what I have on offer...");

            dialogue.AddLine(0, "Time to go.");
            dialogue.AddLine(1, "Good luck, little froglet!");
            dialogue.AddLine(0, "My name is Kiki!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 3;
            dialogue.AddLine(1, "So, what's it like out there?");
            dialogue.AddLine(0, "Like you'd expect. It's WEIRD.");
            dialogue.AddLine(1, "I see. Well, let's see if I have anything that will help you...");

            dialogue.AddLine(1, "Good luck, little froglet!");
            dialogue.AddLine(0, "MY NAME IS- oh, forget it...");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 4;
            dialogue.AddLine(0, "So... How's that potion coming along?");
            dialogue.AddLine(1, "Slowly. The task is more difficult than I anticipated.");
            dialogue.AddLine(0, "Oh. So, you need me to get more flies?");
            dialogue.AddLine(1, "HA HA HA HA! Yes, a LOT more! Did you think we were nearly finished? HA HA HA HA HA!");

            dialogue.AddLine(1, "Good luck, little froglet!");
            dialogue.AddLine(0, "Onwards!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 6;
            dialogue.AddLine(0, "So... This Amulet of Respawning. Where did you get it?");
            dialogue.AddLine(1, "I made it, of course.");
            dialogue.AddLine(0, "Whoa!");
            dialogue.AddLine(1, "That blasted thing has been more trouble than it's worth.");
            dialogue.AddLine(0, "What do you mean?");
            dialogue.AddLine(1, "A tale for another time. ");

            dialogue.AddLine(1, "You're getting good at this. Keep it up, little froglet!");
            dialogue.AddLine(0, "Thank you, Frog Wizard!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 6;
            dialogue.AddLine(0, "I've been meaning to say - I reeeeally like your hat.");
            dialogue.AddLine(1, "Oh my, thank you!");
            dialogue.AddLine(0, "Did you make that, too?");
            dialogue.AddLine(1, "No. A... friend gave it to me.");
            dialogue.AddLine(0, "What a cool friend!");
            dialogue.AddLine(1, "Yes...");

            dialogue.AddLine(0, "Did I say something wrong earlier? About the hat?");
            dialogue.AddLine(1, "...No. It was a special gift, that's all. Go now, little froglet, our time grows short.");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 4;
            dialogue.AddLine(0, "Have you learned anything new about the WEIRDNESS?");
            dialogue.AddLine(1, "What? Oh, yes, my research. Yes, things are coming along swimmingly.");
            dialogue.AddLine(0, "So... what have you learned exactly?");
            dialogue.AddLine(1, "I'm sure the exact details would bore you.");

            dialogue.AddLine(0, "Okay, I've got what I need for now.");
            dialogue.AddLine(1, "Go forth!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 6;
            dialogue.AddLine(0, "You know, I think I'm finally getting used to this respawning thing.");
            dialogue.AddLine(1, "Well don't get TOO used to it. I'll need that Amulet back eventually.");
            dialogue.AddLine(0, "You said the Amulet was more trouble than it's worth. What happened?");
            dialogue.AddLine(1, "An Amulet of such power requires... sacrifice. The materials were not easy to come by and I am still paying the price. The central stone, you see, is imbued with my own life force.");
            dialogue.AddLine(0, "That sounds, like, really serious.");
            dialogue.AddLine(1, "It is.");

            dialogue.AddLine(0, "I will take extra special care of your Amulet, Mr Frog Wizard.");
            dialogue.AddLine(1, "If not, it will take care of you.");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 6;
            dialogue.AddLine(0, "So, the Amulet... it's powered by your... life force?");
            dialogue.AddLine(1, "It is amplified by my life force. I gave the Amulet my youth, and in exchange it gave me the power of second chances.");
            dialogue.AddLine(0, "Whoa. So does that mean you're NOT a really old Frog Wizard?");
            dialogue.AddLine(1, "Correct. Despite my appearance, I am not much older than you are.");
            dialogue.AddLine(0, "Whooooaaaa.");
            dialogue.AddLine(1, "Enough. Weirdness awaits. Take what you need and go.");

            dialogue.AddLine(0, "Maybe this time I won't even need to use the Amulet!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 7;
            dialogue.AddLine(0, "Spoiler alert: I  definitely DID need to use the Amulet.");
            dialogue.AddLine(1, "...");
            dialogue.AddLine(0, "Hey - what's wrong? You look... extra grumpy this time.");
            dialogue.AddLine(1, "I... carry a great burden.");
            dialogue.AddLine(0, "You mean the burden of being the only person who can craft the Potion of UnWeirdness and save Lilytopia?");
            dialogue.AddLine(1, "...I speak of a burden greater even than that.");
            dialogue.AddLine(0, "Whoa.");

            dialogue.AddLine(0, "You wanna talk about it? Your great burden, I mean?");
            dialogue.AddLine(1, "I do not think I could find the words. Go, Kiki, Lilytopia awaits.");
            dialogue.AddLine(0, "OMF YOU SAID MY NAME! I WILL ADVENTURE EXTRA HARD THIS TIME!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 27;
            dialogue.AddLine(1, "Kiki. It's time I told you the truth.");
            dialogue.AddLine(0, "Whoa. The truth about what?");
            dialogue.AddLine(1, "That Amulet you're wearing. And... the Weirdness.");
            dialogue.AddLine(0, "They're... connected?!");
            dialogue.AddLine(1, "In a manner of speaking. I'm afraid the connection between them... is me.");
            dialogue.AddLine(0, "!!!!!!");
            dialogue.AddLine(1, "Many moons ago, I fell in love with a brilliant Frog Librarian.");
            dialogue.AddLine(0, "...");
            dialogue.AddLine(0, "...");
            dialogue.AddLine(0, "That is not what I was expecting.");
            dialogue.AddLine(1, "She was the most studious, hard-working, and highly-organised Frog I ever laid eyes upon.");
            dialogue.AddLine(0, "That's... err... a very sweet story. Kind of. What happened?");
            dialogue.AddLine(1, "We fell in love is what happened! Indeed, our love was so great that I forged the Amulet of Respawning in the hopes that it might never end.");
            dialogue.AddLine(0, "Okay...");
            dialogue.AddLine(1, "As you know, forging the Amulet cost me my youth. My beloved Frog Librarian said I looked too much like her grandpa and ended things.");
            dialogue.AddLine(0, "Whoa. That's cruel. But I guess dating your grandpa is less than ideal.");
            dialogue.AddLine(1, "So, I tried to reverse the effects of the Amulet. I began to weave the Great Spell of Undoing- no easy feat! To my horror, the energies overwhelmed my frail body.");
            dialogue.AddLine(0, "Uh oh.");
            dialogue.AddLine(1, "The Great Spell, unfinished and wild with mystical energies, escaped my grasp. It infested the land with its strangeness. And so you see...");
            dialogue.AddLine(0, "The Weirdness is all your fault.");
            dialogue.AddLine(1, "It is.");
            dialogue.AddLine(0, "And what about the Frog Librarian? Where is she?");
            dialogue.AddLine(1, "She shares the fate of all Lilytopians. She has gone Weird.");
            dialogue.AddLine(0, "Daamn. That's, like, some classic tragedy stuff right there.");
            dialogue.AddLine(1, "Indeed it is. I thank you, Kiki, for hearing my tale.");
            dialogue.AddLine(0, "Thank you for sharing it!");
            dialogue.AddLine(1, "Is there anything more I can do to help you?");

            dialogue.AddLine(0, "Wish me luck.");
            dialogue.AddLine(1, "Always, Kiki. Always.");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 3;
            dialogue.AddLine(0, "So... any other shocking revelations to share with me?");
            dialogue.AddLine(1, "No. You have heard all that there is to hear. From now on, we shall focus on the task at hand.");
            dialogue.AddLine(0, "Okay. Well, I'll just keep collecting flies I guess!");

            dialogue.AddLine(1, "Good luck out there!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 2;
            dialogue.AddLine(0, "I gotta say... I think I'm getting pretty good at all this.");
            dialogue.AddLine(1, "I have to admit, you really are.");
            dialogue.AddLine(0, "Thanks Mr Frog Wizard!");
            dialogue.AddLine(1, "You're welcome, Kiki the frog!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 4;
            dialogue.AddLine(0, "So. Your hat. Did she give it to you?");
            dialogue.AddLine(1, "She did.");
            dialogue.AddLine(0, "You look good in it.");
            dialogue.AddLine(1, "Thank you!");
            dialogue.AddLine(0, "Time to eat some flies!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 4;
            dialogue.AddLine(0, "So... what do NORMAL flies taste like? Just realised I've only tasted the Weird ones.");
            dialogue.AddLine(1, "Well, according to my research, Weirdness is a very bitter compound, so I think you're going to LOVE normal flies when you try them.");
            dialogue.AddLine(0, "If we ever see them again.");
            dialogue.AddLine(1, "Indeed.");
            dialogue.AddLine(0, "Ready to go!");
            dialogue.AddLine(1, "Have fun!");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 2;
            dialogue.AddLine(1, "Good run?");
            dialogue.AddLine(0, "See for yourself...");
            dialogue.AddLine(1, "Ready to give it another go?");
            dialogue.AddLine(0, "Haven't got anything else to do!");
            dialogue.AddLine(1, "Good point.");

            Dialogues
                .Add(dialogue);

            dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.OpenShopLine = 1;
            dialogue.AddLine(1, "See anything you like?");
            dialogue.AddLine(1, "Good luck out there!");

            Dialogues
                .Add(dialogue);
        }

        #endregion

        #region Unity

        protected void OnEnable()
        {
            PlayLooper
                .EnsureNotPlaying();

            MenuLooper
               .EnsurePlaying();

            MenuLooper
                .MoveToLoop(1);

            PauseGame(true);

            StartDialogue();
        }

        protected override void Awake()
        {
            base
                .Awake();

            LineToType = string.Empty;

            DialogueIndex = 0;

            PlayLooper = FindObjectOfType<PlayMusicLooper>();

            MenuLooper = FindObjectOfType<MenuMusicLooper>();            
                        
            CreateDialogues();            
        }

        protected void TypeText()
        {
            if (LineTypeIndex > LineToType.Length)
            {
                return;
            }

            DialogueText.text = LineToType.Substring(0, LineTypeIndex);
        }

        protected void Update()
        {
            if (LineTypeIndex <= LineToType.Length)
            {
                if (LetterCountdown > 0)
                {
                    LetterCountdown -= Time.deltaTime;
                    if (LetterCountdown  <= 0)
                    {
                        LetterCountdown = LetterSpeed;
                        LineTypeIndex++;

                        TypeText();                        
                    }
                }

                PlaySpeakerVoice();
            }            

            if (Input.GetKeyDown(KeyCode.Return))
            {
                AdvanceDialogueText();
            }            
        }

        #endregion
    }
}
