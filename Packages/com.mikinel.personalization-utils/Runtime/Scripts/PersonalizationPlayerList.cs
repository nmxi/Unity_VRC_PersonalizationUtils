
using UdonSharp;

namespace mikinel.vrc.PersonalizationUtils
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PersonalizationPlayerList : UdonSharpBehaviour
    {
        public string[] playerList; //display name of players
    }
}