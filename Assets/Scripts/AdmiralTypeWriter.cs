using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AdmiralTypeWriter : MonoBehaviour
{
    public string test_word; 
    public string CurrentText="";
    public float ProgressComplete;
   
    INPUTS Typewriter_Inputs;


    CancellationTokenSource _tokenSource = null;



    private void Awake() {
        INPUTS thisuser= new INPUTS();
        Typewriter_Inputs= thisuser;
        Typewriter_Inputs.Enable();
        Typewriter_Inputs.Typewriter.Enable();

        Typewriter_Inputs.Typewriter.stop.performed+= StopEffect;

    }

      async void Start()
    {

        await Task.Run(()=> ReadingTask());
        
    }
   
    async void ReadingTask(){
        // progress of tasks
        var progress = new Progress<int>(value => {
          ProgressComplete= value;
        });
        // make token to pass to task
          _tokenSource = new CancellationTokenSource();
          var token = _tokenSource.Token;
        
        await Task.Run(()=>Reading(progress,test_word,token));

    }


    

     void Reading(IProgress<int> progress, string word, CancellationToken token){
       /* can use in async
       while (CurrentText.Length< test_word.Length)
       {
           CurrentText+= "a";
           yield return new WaitForSeconds(Random.Range(1f,2f));
           print(CurrentText);
       }
       */
      if(word!=null){
      for(int x =0; x<word.Length;x++){
        CurrentText+= word[x].ToString();
        Task.Delay(200).Wait();
        print(CurrentText);
       
        var compeletion = (CurrentText.Length/word.Length)*100;
        progress.Report(compeletion);


        if(token.IsCancellationRequested)
         {
             print("done reading");
             CurrentText = test_word;
             return;
         }
      }
     
     }
       
    }
      private void StopEffect(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
         _tokenSource.Cancel();
    }




}
