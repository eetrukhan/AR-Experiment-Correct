using System;
using System.Collections.Generic;

[Serializable]
public class TrialData
{
    public uint SubjectNo;
    public float IntensityOfObstacleAppearance; // Obstacles per meter
    public uint Design;

    public string Procedure;
    public uint TrialNo;

    public bool Success;

    public float Time; // In seconds
    public float TrackLength; // In meters
    public float ActualPathLength; // The same

    public uint TotalNumberOfGroundObstacles;
    public uint NumberOfGroundObstaclesTouched;
    public uint TotalNumberOfHighObstacles;
    public uint NumberOfHighObstaclesTouched;
    public uint TotalNumberOfObstacles
    {
        get { return TotalNumberOfGroundObstacles + TotalNumberOfHighObstacles; }
    }
    public uint NumberOfObstaclesTouched
    {
        get { return NumberOfGroundObstaclesTouched + NumberOfHighObstaclesTouched; }
    }

    public uint TrackOverruns;

    public uint CollisionsNumberReported;
    public string RealCollisionsNumber = "0";

    public float SphereVelocity;

    public string Note;

    // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
    private static readonly string _formURI = "https://docs.google.com/forms/d/e/1FAIpQLSeVkrC-gpVbrTfClTp5ukyqmQHiwYDOm6BhQizuxdIAplZmzg/formResponse";

    public static string GetFormURI()
    {
        return _formURI;
    }

    public Dictionary<string, string> GetFormFields()
    {
        Dictionary<string, string> formFields = new Dictionary<string, string>();
        formFields.Add("entry.10067330", SubjectNo.ToString());
        formFields.Add("entry.855995704", IntensityOfObstacleAppearance.ToString());
        formFields.Add("entry.2123360511", Design.ToString());

        if (Procedure != null) formFields.Add("entry.2112534605", Procedure);
        formFields.Add("entry.505109796", TrialNo.ToString());

        formFields.Add("entry.536203045", Success.ToString());

        formFields.Add("entry.1476769614", Time.ToString());
        formFields.Add("entry.713667275", TrackLength.ToString());
        formFields.Add("entry.536480807", ActualPathLength.ToString());

        formFields.Add("entry.601887342", TotalNumberOfGroundObstacles.ToString());
        formFields.Add("entry.1505997844", NumberOfGroundObstaclesTouched.ToString());
        formFields.Add("entry.1081510321", TotalNumberOfHighObstacles.ToString());
        formFields.Add("entry.328739628", NumberOfHighObstaclesTouched.ToString());

        formFields.Add("entry.942971647", TrackOverruns.ToString());

        formFields.Add("entry.1999166916", CollisionsNumberReported.ToString());
        formFields.Add("entry.273234747", RealCollisionsNumber);

        formFields.Add("entry.1722113900", SphereVelocity.ToString());

        if (Note != null) formFields.Add("entry.2068613620", Note);

        return formFields;
    }
}
