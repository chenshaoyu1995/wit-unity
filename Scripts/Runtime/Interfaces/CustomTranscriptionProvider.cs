﻿/*
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using com.facebook.witai.events;
using UnityEngine;
using UnityEngine.Events;

namespace com.facebook.witai.interfaces
{
    public abstract class CustomTranscriptionProvider : MonoBehaviour, ITranscriptionProvider
    {
        [SerializeField] private bool overrideMicLevel = false;

        private WitTranscriptionEvent onPartialTranscription = new WitTranscriptionEvent();
        private WitTranscriptionEvent onFullTranscription = new WitTranscriptionEvent();
        private UnityEvent onStoppedListening = new UnityEvent();
        private UnityEvent onStartListening = new UnityEvent();
        private WitMicLevelChangedEvent onMicLevelChanged = new WitMicLevelChangedEvent();

        public string LastTranscription { get; }
        public WitTranscriptionEvent OnPartialTranscription => onPartialTranscription;
        public WitTranscriptionEvent OnFullTranscription => onFullTranscription;
        public UnityEvent OnStoppedListening => onStoppedListening;
        public UnityEvent OnStartListening => onStartListening;
        public WitMicLevelChangedEvent OnMicLevelChanged => onMicLevelChanged;
        public bool OverrideMicLevel => overrideMicLevel;

        public abstract void Activate();
        public abstract void Deactivate();
    }
}
