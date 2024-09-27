using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TimerUI :NetworkBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    [Networked] public TickTimer LimitTimer { get; set; }

    [Networked] public float TimeSec { get; set; }
    public UnityEvent EndTimerEvent;

    public override void Spawned()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void TurnOn(float timeLimit)
    {
        TimeSec = timeLimit;
        LimitTimer = TickTimer.CreateFromSeconds(Runner, timeLimit);
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        UpdateTimerRpc();

        if (LimitTimer.Expired(Runner))
        {
            EndTimerEvent.Invoke();

            LimitTimer = TickTimer.None;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void UpdateTimerRpc()
    {
        _textMeshPro.text = ((int)LimitTimer.RemainingTime(Runner)).ToString();
    }
}
