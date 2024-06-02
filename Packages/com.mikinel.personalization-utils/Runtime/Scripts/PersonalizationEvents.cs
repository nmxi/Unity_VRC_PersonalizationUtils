using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace mikinel.vrc.PersonalizationUtils
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PersonalizationEvents : UdonSharpBehaviour
    {
        [SerializeField] private PersonalizationPlayerList _personalizationPlayerList;
        [SerializeField] private Mode _mode;
        
        [Space]
        [SerializeField] private GameObject[] _enableObjectArray;
        [SerializeField] private GameObject[] _disableObjectArray;
        
        private bool _isFirstTime = true;
        private bool _prevState;
        
        // [Space]
        // [SerializeField] private Animator[] _animatorStateControlTargetArray;
        // [SerializeField] private string[] _animationStateControlParameterArray;
        //
        // [Space]
        // [SerializeField] private Animator[] _animatorTriggerControlTargetArray;
        // [SerializeField] private string[] _animationTriggerControlParameterArray;
        
        private const string _debugLogPrefix = "<color=yellow>[Personalization Utils]</color> ";
        
        // private bool ValidateSettings()
        // {
        //     if (_isAnimationStateControlEnabled)
        //     {
        //         if (_animatorStateControlTargetArray == null || _animationStateControlParameterArray == null ||
        //             _animatorStateControlTargetArray.Length != _animationStateControlParameterArray.Length)
        //         {
        //             Debug.LogError($"{_debugLogPrefix}AnimatorStateControl settings are invalid.");
        //             return false;
        //         }
        //     }
        //     
        //     if (_isAnimationTriggerControlEnabled)
        //     {
        //         if (_animatorTriggerControlTargetArray == null || _animationTriggerControlParameterArray == null ||
        //             _animatorTriggerControlTargetArray.Length != _animationTriggerControlParameterArray.Length)
        //         {
        //             Debug.LogError($"{_debugLogPrefix}AnimatorTriggerControl settings are invalid.");
        //             return false;
        //         }
        //     }
        //     
        //     return true;
        // }
        
        private void InvokeObjectControlEvent(bool isEnable)
        {
            foreach (var obj in _enableObjectArray)
            {
                obj.SetActive(isEnable);
            }

            foreach (var obj in _disableObjectArray)
            {
                obj.SetActive(!isEnable);
            }
        }
        
        // private void InvokeAnimationStateControlEvent(bool isEnable)
        // {
        //     for (var i = 0; i < _animatorStateControlTargetArray.Length; i++)
        //     {
        //         _animatorStateControlTargetArray[i].SetBool(_animationStateControlParameterArray[i], isEnable);
        //     }
        // }
        //
        // private void InvokeAnimationTriggerControlEvent()
        // {
        //     for (var i = 0; i < _animatorTriggerControlTargetArray.Length; i++)
        //     {
        //         _animatorTriggerControlTargetArray[i].SetTrigger(_animationTriggerControlParameterArray[i]);
        //     }
        // }

        private void Start()
        {
            OnUpdatedPlayerList();  //initialize
        }

        public void OnUpdatedPlayerList()
        {
            var currentPlayers = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
            VRCPlayerApi.GetPlayers(currentPlayers);
            var currentPlayerDisplayNames = new string[currentPlayers.Length];
            for (var i = 0; i < currentPlayers.Length; i++)
            {
                currentPlayerDisplayNames[i] = currentPlayers[i].displayName;
            }
            
            switch(_mode)
            {
                //リスト内の誰か1人でも居るときに有効
                case Mode.OR:
                    foreach (var playerDisplayName in currentPlayerDisplayNames)
                    {
                        //リストの中の人が現在のワールドに居たとき
                        if (Array.IndexOf(_personalizationPlayerList.playerList, playerDisplayName) != -1)
                        {
                            InvokeEvent(true);
                            return;
                        }
                    }
                    InvokeEvent(false);
                    break;
                
                //リスト内の全員が居るときに有効
                case Mode.AND:
                    foreach (var targetPlayer in _personalizationPlayerList.playerList)
                    {
                        //リストの中の人が現在のワールドに居なかったとき
                        if (Array.IndexOf(currentPlayerDisplayNames, targetPlayer) == -1)
                        {
                            InvokeEvent(false);
                            return;
                        }
                    }
                    InvokeEvent(true);
                    break;
                
                //リスト内に含まれている人以外が居るときに無効
                case Mode.Whitelist:
                    foreach (var playerDisplayName in currentPlayerDisplayNames)
                    {
                        //リストに含まれてない人が居たとき
                        if (Array.IndexOf(_personalizationPlayerList.playerList, playerDisplayName) == -1)
                        {
                            InvokeEvent(false);
                            return;
                        }
                    }
                    InvokeEvent(true);
                    break;
            }
        }
        
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            SendCustomEventDelayedFrames(nameof(OnUpdatedPlayerList), 1);
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            SendCustomEventDelayedFrames(nameof(OnUpdatedPlayerList), 1);
        }

        private void InvokeEvent(bool isEnable)
        {
            Debug.Log($"{_debugLogPrefix}InvokeEvent: {isEnable}");
            
            if (!_isFirstTime && _prevState == isEnable) return; //変化がない場合は何もしない
            
            InvokeObjectControlEvent(isEnable);
            // InvokeAnimationStateControlEvent(isEnable);
            // InvokeAnimationTriggerControlEvent();
            
            _isFirstTime = false;
            _prevState = isEnable;
        }
    }
    
    public enum Mode
    {
        //リスト内の誰か1人でも居るときに有効
        OR = 0,
        
        //リスト内の全員が居るときに有効
        AND = 1,
        
        //リスト内に含まれている人以外が居るときに無効
        Whitelist = 2,
    }
}