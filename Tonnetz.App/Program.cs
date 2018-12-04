using System;
using System.Collections.Generic;
using System.Linq;

namespace Tonnetz.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Give me chord one!");
            //var chord1 = "C1 E1 G1 B1";//Console.ReadLine();
            Console.WriteLine("Give me chord two!");
            //var chord2 = "F1 A1 C1 E2";//Console.ReadLine();

            NoteHelper.CompareChordsNotes("C1 E1 G1 B1", "F1 A1 C1 E2");
            
            Console.WriteLine("");
            
            NoteHelper.CompareChordsNotes("C1 B1 E2 G2", "F1 C1 E2 A2");

            Console.WriteLine("");
            
            NoteHelper.CompareChordsNotes("C1 B1 E2 G2", "F1 E2 A2 C3");
            
            Console.WriteLine("G7, Cmaj7");
            
            NoteHelper.CompareChordsNotes("G1 B1 D2 F2", "C1 E1 G1 B1");

            Console.WriteLine("G7");
            

            NoteHelper.CompareChordNotes("G1 D2 F2 B2");
            Console.WriteLine("G7, Cmaj7"); 
            NoteHelper.CompareChordsNotes("G1 D2 F2 B2", "C1 B1 E2 G2");
            
            //resolving with h note as aug lead to unpleasent resolvment.




            var startNote = NoteHelper.GetNote("C1");

            for (int i = 0; i < 20; i++)
            {
                Console.Write(startNote + " => +5 => ");
                startNote = startNote.GoByInterval(Interval.Perfect5);
            }
            Console.Read();
        }

        
    }

    public enum Interval
    {
        None = 0,
        Minor2 = 1,
        Major2 = 2,
        Minor3 = 3,
        Major3 = 4,
        Perfect4 = 5,
        Aug4 = 6,
        Perfect5 = 7,
        Minor6 = 8,
        Major6 = 9,
        Minor7 = 10,
        Major7 = 11,
        Octave = 12
    }

    public class Note
    {
        public string Name { get; set; }
        public int Octave { get; set; }
        public int Step { get; set; }

        public int SemitonesFromZero {get {return this.Octave * (int)Interval.Octave + this.Step;}}

        public override string ToString()
        {
            return Name + Octave;
        }

        public Note GoByInterval( Interval interval)
        {
            var newStep = this.Step + (int)interval;
            var newOctave = this.Octave;
            if (newStep >= (int) Interval.Octave)
            {
                newStep = newStep - (int)Interval.Octave;
            }

            return NoteHelper.Notes.SingleOrDefault(n => n.Octave == newOctave && n.Step == newStep);
        }
        
        public static Interval operator- (Note one, Note two)
       {
           if (one.SemitonesFromZero > two.SemitonesFromZero)
           {
              return (Interval)((one.SemitonesFromZero - two.SemitonesFromZero) % (int)Interval.Octave);
           }
           else
           {
              return (Interval)((two.SemitonesFromZero - one.SemitonesFromZero) % (int)Interval.Octave); 
           }
       }
        
    }
    
    public static class NoteHelper
    {
        public static void CompareChordsNotes(string chord1, string chord2)
        {
            var chordOneNotes = chord1.Split(" ").Select(s => NoteHelper.GetNote(s)).ToArray();
            var chordTwoNotes = chord2.Split(" ").Select(s => NoteHelper.GetNote(s)).ToArray();

            for (int i = 0; i < chordOneNotes.Count(); i++)
            {
                Console.WriteLine(chordOneNotes[i] + "-" + chordTwoNotes[i] + "=" + (Interval)(chordOneNotes[i] - chordTwoNotes[i]));
            }
        }
        
        public static void CompareChordNotes(string chord)
        {
            var chordOneNotes = chord.Split(" ").Select(s => NoteHelper.GetNote(s)).ToArray();
            

            for (int i = 0; i < chordOneNotes.Count()-1; i++)
            {
                Console.WriteLine(chordOneNotes[i] + "-" + chordOneNotes[i+1] + "=" + (Interval)(chordOneNotes[i] - chordOneNotes[i+1]));
            }
        }

        public static Dictionary<Interval, string> NoteNameBySteps { get; set; } =
            new Dictionary<Interval, string>()
            {
                {Interval.None, "C"},
                {Interval.Minor2, "C#"},
                {Interval.Major2, "D"},
                {Interval.Minor3, "D#"},
                {Interval.Major3, "E"},
                {Interval.Perfect4, "F"},
                {Interval.Aug4, "F#"},
                {Interval.Perfect5, "G"},
                {Interval.Minor6, "G#"},
                {Interval.Major6, "A"},
                {Interval.Minor7, "A#"},
                {Interval.Major7, "B"},
                {Interval.Octave, "Co"},
            };
    
        public static List<Note> Notes { get; set; } = GenerateNotes();
        
        private static List<Note> GenerateNotes()
        {
            List<Note> list = new List<Note>();
            for (int octave = 0; octave < 6; octave++)
            {
                for (int step = 0; step < 12; step++)
                {
                    list.Add(new Note(){Name = NoteNameBySteps[(Interval)step], Octave = octave, Step = step});
                }
            }

            return list;
        }

        public static Note GetNote(string noteName)
        {
            return Notes.SingleOrDefault(n => n.ToString().Equals(noteName));
        }

       
    
    }


}