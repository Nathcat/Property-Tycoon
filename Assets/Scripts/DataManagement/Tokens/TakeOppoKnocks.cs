using System.Collections;
using UnityEngine;

public class TakeOppoKnocks : Command
{
    public TakeOppoKnocks(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Debug.Log(counterController.name + " plays an Opportunity Knocks card");
        yield return GameController.instance.DrawOpportunity(counterController).action.Run(counterController);
<<<<<<< Updated upstream
        Settings.SetActive(true);
=======
        Settings.SetActive(true); 
>>>>>>> Stashed changes
        public void MMSettingsButton()
        {
            StartScreen.SetActive(false);
<<<<<<< Updated upstream
            Settings.SetActive(true);
=======
        Settings.SetActive(true); 
>>>>>>> Stashed changes
            yield break;
    }
}
