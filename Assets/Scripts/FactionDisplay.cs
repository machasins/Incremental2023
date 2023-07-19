using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionDisplay : MonoBehaviour
{
    public SpriteRenderer anime;
    public SpriteRenderer furry;
    public SpriteRenderer streamer;
    public SpriteRenderer military;
    public int maxPoints;

    [HideInInspector] public int[] factionPoints = new int[(int)User.Type.maxUserType];

    public void AddFactionPoints(string type)
    {
        User.Type faction = (User.Type)System.Enum.Parse(typeof(User.Type), type);

        switch (faction)
        {
            case User.Type.normal:
                factionPoints[(int)faction]++;
                streamer.color = Color.Lerp(Color.clear, Color.white, (float)factionPoints[(int)faction] / maxPoints);
                break;
            case User.Type.anime:
                factionPoints[(int)faction]++;
                anime.color = Color.Lerp(Color.clear, Color.white, (float)factionPoints[(int)faction] / maxPoints);
                break;
            case User.Type.furry:
                factionPoints[(int)faction]++;
                furry.color = Color.Lerp(Color.clear, Color.white, (float)factionPoints[(int)faction] / maxPoints);
                break;
            case User.Type.kommando:
                factionPoints[(int)faction]++;
                military.color = Color.Lerp(Color.clear, Color.white, (float)factionPoints[(int)faction] / maxPoints);
                break;
        }
    }
}
