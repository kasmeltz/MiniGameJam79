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

        public TMP_Text DialogueText;

        public Image SpeakerBubble;

        public Button AdvanceTextButton;

        public int DialogueIndex { get; set; } 

        protected List<Dialogue> Dialogues { get; set; }

        protected Dialogue Dialogue { get; set; }

        protected int DialogueLineIndex { get; set; }

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

        public void StartDialogue()
        {
            if (DialogueIndex >= Dialogues.Count)
            {
                return;
            }

            Dialogue = Dialogues[DialogueIndex];

            DialogueLineIndex = 0;

            AdvanceTextButton
                .gameObject
                .SetActive(true);

            ShowDialogue();
        }

        public void NextDialogue()
        {
            DialogueIndex++;
            StartDialogue();
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

            if (DialogueLineIndex >= Dialogue.Lines.Count)
            {
                AdvanceTextButton
                    .gameObject
                    .SetActive(false);

                OnDialogueCompleted();

                return;
            }

            ShowDialogue();
        }

        #endregion

        #region Protected Methods

        protected void ShowDialogue()
        {
            var line = Dialogue.Lines[DialogueLineIndex];
            DialogueText.text = line.Text;

            var scale = new Vector3(1, 1, 1);
            if (line.SpeakerIndex == 0)
            {
                scale.x = -1;                
            }

            SpeakerBubble.transform.localScale = scale;
            DialogueText.transform.localScale = scale;
        }

        protected void CreateDialogues()
        {
            Dialogues = new List<Dialogue>();

            var dialogue = new Dialogue("Kiki", "Frog Wizard");
            dialogue.AddLine(0, "Erm. What is going on here? This place looks bananas!");
            dialogue.AddLine(1, "Indeed! A terrible Weirdness has consumed Lilytopia!");
            dialogue.AddLine(0, "A 'Weirdness' you say? I guess that's... a bad thing?");
            dialogue.AddLine(1, "A VERY bad thing! Everyone has lost their minds!");
            dialogue.AddLine(0, "Everyone... except us?");
            dialogue.AddLine(1, "Looks like it.");
            dialogue.AddLine(0, "So we're screwed.");
            dialogue.AddLine(1, "Not quite. I have a plan. I am a Great and Powerful Frog Wizard, after all.");
            dialogue.AddLine(0, "Oh cool!");
            dialogue.AddLine(1, "I shall concoct a great Potion of Un-Weirdness to restore Lilytopia to its former glory!");
            dialogue.AddLine(0, "Aww, yes!");
            dialogue.AddLine(1, "But I need your help.");
            dialogue.AddLine(0, "Aww, no!");
            dialogue.AddLine(1, "I must study the Weirdness to determine its mysterious properties. To do this, I'll need flies. LOTS of them. And I'll need you to get them for me.");
            dialogue.AddLine(0, "But... I only just got here! Plus I'm only five minutes old!");
            dialogue.AddLine(1, "Yet it seems the Frogs of Fate have chosen you. ");
            dialogue.AddLine(0, "Whoa! I love the Frogs of Fate!");
            dialogue.AddLine(0, "But isn't this, like, reeeeeally dangerous?");
            dialogue.AddLine(1, "Take this. My most precious relic - THE AMULET OF RESPAWNING! Whenever you die, you will instantly respawn right here.");
            dialogue.AddLine(0, "Whenever I WHAT?!");
            dialogue.AddLine(1, "I'm sure it's not as bad as it sounds. Now go! And good luck, little froglet!");
            dialogue.AddLine(0, "The name's Kiki.");
            dialogue.AddLine(1, "Whatever! Goodbye!");

            Dialogues
                .Add(dialogue);

            /*


LOOP 2
KIKI
That. Was. GROSS! Dying is the absolute WORST!
FROG WIZARD
So I've heard. But we're one step closer to an antidote to this Weirdness. 
KIKI
Well, okay then... 
FROG WIZARD
There is something I should tell you. This is not my home- it's a shop!
KIKI
Capitalism?! At a time like this?
FROG WIZARD
The flies you bring me can be used to craft magical items and potions that will help you survive the Weirdness. 
KIKI
Oh, actually yes, I would like some of those. 
FROG WIZARD
Here's what I have on offer...

//SHOPPING SEQUENCE//

KIKI
Time to go.
FROG WIZARD
Good luck, little froglet!
KIKI
My name is Kiki!

LOOP 3

FROG WIZARD
So, what's it like out there?
KIKI
Like you'd expect. It's WEIRD.
FROG WIZARD
I see. Well, let's see if I have anything that will help you...

//SHOPPING SEQUENCE//

FROG WIZARD
Good luck, little froglet!
KIKI
MY NAME IS- oh, forget it...

LOOP 4

KIKI
So... How's that potion coming along?
FROG WIZARD
Slowly. The task is more difficult than I anticipated. 
KIKI
Oh. So, you need me to get more flies?
FROG WIZARD
HA HA HA HA! Yes, a LOT more! Did you think we were nearly finished? HA HA HA HA HA!

//SHOPPING SEQUENCE//

FROG WIZARD
Good luck, little froglet!
KIKI
Onwards!

LOOP 5

KIKI
So... This Amulet of Respawning. Where did you get it?
FROG WIZARD
I made it, of course.  
KIKI
Whoa!
FROG WIZARD
That blasted thing has been more trouble than it's worth.
KIKI
What do you mean?
FROG WIZARD
A tale for another time. 

//SHOPPING SEQUENCE//
FROG WIZARD
You're getting good at this. Keep it up, little froglet!
KIKI
Thank you, Frog Wizard!

LOOP 6

KIKI
I've been meaning to say - I reeeeally like your hat.
FROG WIZARD
Oh my, thank you!
KIKI
Did you make that, too?

FROG WIZARD
No. A... friend gave it to me.
KIKI
What a cool friend!
FROG WIZARD
Yes...
//SHOPPING SEQUENCE//

KIKI
Did I say something wrong earlier? About the hat?
FROG WIZARD
...No. It was a special gift, that's all. Go now, little froglet, our time grows short. 

LOOP 7

KIKI
Have you learned anything new about  the WEIRDNESS?
FROG WIZARD
What? Oh, yes, my research. Yes, things are coming along swimmingly. 
KIKI
So... what have you learned exactly?
FROG WIZARD
I'm sure the exact details would bore you. 

//SHOPPING SEQUENCE//

KIKI
Okay, I've got what I need for now.
FROG WIZARD
Go forth!

LOOP 8

KIKI
You know, I think I'm finally getting used to this respawning thing.  
FROG WIZARD
Well don't get TOO used to it. I'll need that Amulet back eventually. 
KIKI
You said the Amulet was more trouble than it's worth. What happened?
FROG WIZARD
An Amulet of such power requires... sacrifice. The materials were not easy to come by and I am still paying the price. The central stone, you see, is imbued with my own life force.
KIKI
That sounds, like, really serious.
FROG WIZARD
It is.

//SHOPPING SEQUENCE//

KIKI
I will take extra special care of your Amulet, Mr Frog Wizard.
KIKI
If not, it will take care of you. 

LOOP 9

KIKI
So, the Amulet... it's powered by your... life force?
FROG WIZARD
It is amplified by my life force. I gave the Amulet my youth, and in exchange it gave me the power of second chances. 
KIKI
Whoa. So does that mean you're NOT a really old Frog Wizard?
FROG WIZARD
Correct. Despite my appearance, I am not much older than you are. 
KIKI
Whooooaaaa. 
FROG WIZARD
Enough. Weirdness awaits. Take what you need and go. 

//SHOPPING SEQUENCE//

KIKI
Maybe this time I won't even need to use the Amulet!


LOOP 10
KIKI
Spoiler alert: I  definitely DID need to use the Amulet.
...
Hey - what's wrong? You look... extra grumpy this time.
FROG WIZARD
I... carry a great burden.
KIKI
You mean the burden of being the only person who can craft the Potion of UnWeirdness and save Lilytopia?
FROG WIZARD
...I speak of a burden greater even than that. 
KIKI
Whoa. 

//SHOPPING SEQUENCE//

KIKI
You wanna talk about it? Your great burden, I mean?
FROG WIZARD
I do not think I could find the words. Go, Kiki, Lilytopia awaits. 
KIKI
OMF YOU SAID MY NAME! I WILL ADVENTURE EXTRA HARD THIS TIME!


LOOP 11 - NARRATIVE ENDS HERE
FROG WIZARD
Kiki. It's time I told you the truth.
KIKI
Whoa. The truth about what?
FROG WIZARD
That Amulet you're wearing. And... the Weirdness. 
KIKI
They're... connected?!
FROG WIZARD
In a manner of speaking. I'm afraid the connection between them... is me.
KIKI
!!!!!!
FROG WIZARD
Many moons ago, I fell in love with a brilliant Frog Librarian.
KIKI
... 
... 
That is not what I was expecting.

FROG WIZARD
She was the most studious, hard-working, and highly-organised Frog I ever laid eyes upon. 
KIKI
That's... err... a very sweet story. Kind of. What happened?

FROG WIZARD
We fell in love is what happened! Indeed, our love was so great that I forged the Amulet of Respawning so that it would never end.
KIKI
Okay...
FROG WIZARD
As you know, forging the Amulet cost me my youth. My beloved Frog Librarian said I looked too much like her grandpa and ended things.
KIKI
Whoa. That's cruel. But I guess dating your grandpa is less than ideal. 

FROG WIZARD
So, I tried to reverse the effects of the Amulet. I began to weave the Great Spell of Undoing- no easy feat! To my horror, the energies overwhelmed my frail body. 

 KIKI
Uh oh.
FROG WIZARD
The Great Spell, unfinished and wild with mystical energies, escaped my grasp. It infested the land with its strangeness. And so you see...
KIKI
The Weirdness is all your fault.
FROG WIZARD
It is.
KIKI
And what about the Frog Librarian? Where is she?
FROG WIZARD
She shares the fate of all Lilytopians. She has gone Weird. 
KIKI
Daamn. That's, like, some classic tragedy stuff right there. 
FROG WIZARD
Indeed it is. I thank you, Kiki, for hearing my tale. 
KIKI
Thank you for sharing it!
FROG WIZARD
Is there anything more I can do to help you?

//SHOPPING SEQUENCE//

KIKI
Wish me luck.
FROG WIZARD
Always, Kiki. Always.


LOOP 12
		KIKI
So... any other shocking revelations to share with me?
FROG WIZARD
Nope. You're all caught up. 
KIKI
Okay. Well, I'll just keep collecting flies I guess!

//SHOPPING SEQUENCE//
FROG WIZARD
Good luck out there!

LOOP 13
KIKI
I gotta say... I think I'm getting pretty good at all this.
FROG WIZARD
I have to admit, you really are.


//SHOPPING SEQUENCE//

KIKI
Thanks Mr Frog Wizard!
FROG WIZARD
You're welcome, Kiki the frog!


LOOP 14
KIKI
So. Your hat. Did she give it to you?
FROG WIZARD
She did. 
KIKI
You look good in it.
FROG WIZARD
Thank you!
//SHOPPING SEQUENCE//
KIKI
Time to eat some flies!



LOOP 15
KIKI
So... what do NORMAL flies taste like? Just realised I've only tasted the Weird ones.

FROG WIZARD
Well, according to my research, Weirdness is a very bitter compound, so I think you're going to LOVE normal flies when you try them. 
KIKI
If we ever see them again.
FROG WIZARD
Indeed. 


//SHOPPING SEQUENCE//

KIKI
Ready to go!
FROG WIZARD
Have fun!

LOOP 16
FROG WIZARD
Good run?
KIKI
See for yourself...
//SHOPPING SEQUENCE//
FROG WIZARD
Ready to give it another go?
KIKI
Haven't got anything else to do!
FROG WIZARD
Good point. 

LOOP 17 - GENERIC TEXT

FROG WIZARD
See anything you like?

//SHOPPING SEQUENCE//

FROG WIZARD
Good luck out there!

            */

        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            DialogueIndex = 0;

            CreateDialogues();
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AdvanceDialogueText();
            }
        }

        #endregion
    }
}