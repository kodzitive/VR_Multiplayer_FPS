using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class GameModel
{
    [RealtimeProperty(1, true, true)]
    private bool _gameInitialized;

    [RealtimeProperty(2, true, true)]
    private int _score;
}
/* ----- Begin Normal Autogenerated Code ----- */
public partial class GameModel : IModel {
    // Properties
    public bool gameInitialized {
        get { return _cache.LookForValueInCache(_gameInitialized, entry => entry.gameInitializedSet, entry => entry.gameInitialized); }
        set { if (value == gameInitialized) return; _cache.UpdateLocalCache(entry => { entry.gameInitializedSet = true; entry.gameInitialized = value; return entry; }); FireGameInitializedDidChange(value); }
    }
    public int score {
        get { return _cache.LookForValueInCache(_score, entry => entry.scoreSet, entry => entry.score); }
        set { if (value == score) return; _cache.UpdateLocalCache(entry => { entry.scoreSet = true; entry.score = value; return entry; }); FireScoreDidChange(value); }
    }
    
    // Events
    public delegate void GameInitializedDidChange(GameModel model, bool value);
    public event         GameInitializedDidChange gameInitializedDidChange;
    public delegate void ScoreDidChange(GameModel model, int value);
    public event         ScoreDidChange scoreDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool gameInitializedSet;
        public bool gameInitialized;
        public bool scoreSet;
        public int  score;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    public GameModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireGameInitializedDidChange(bool value) {
        try {
            if (gameInitializedDidChange != null)
                gameInitializedDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    public void FireScoreDidChange(int value) {
        try {
            if (scoreDidChange != null)
                scoreDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        GameInitialized = 1,
        Score = 2,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _gameInitialized = gameInitialized;
            _score = score;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteVarint32Length((uint)PropertyID.GameInitialized, _gameInitialized ? 1u : 0u);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.Score, (uint)_score);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.gameInitializedSet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.GameInitialized, entry.gameInitialized ? 1u : 0u);
                if (entry.scoreSet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.Score, (uint)entry.score);
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteVarint32((uint)PropertyID.GameInitialized, _gameInitialized ? 1u : 0u);
            stream.WriteVarint32((uint)PropertyID.Score, (uint)_score);
        } else {
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.gameInitializedSet || entry.scoreSet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.gameInitializedSet)
                    stream.WriteVarint32((uint)PropertyID.GameInitialized, entry.gameInitialized ? 1u : 0u);
                if (entry.scoreSet)
                    stream.WriteVarint32((uint)PropertyID.Score, (uint)entry.score);
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool gameInitializedExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.gameInitializedSet);
        bool scoreExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.scoreSet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.GameInitialized: {
                    bool previousValue = _gameInitialized;
                    
                    _gameInitialized = (stream.ReadVarint32() != 0);
                    
                    if (!gameInitializedExistsInChangeCache && _gameInitialized != previousValue)
                        FireGameInitializedDidChange(_gameInitialized);
                    break;
                }
                case (uint)PropertyID.Score: {
                    int previousValue = _score;
                    
                    _score = (int)stream.ReadVarint32();
                    
                    if (!scoreExistsInChangeCache && _score != previousValue)
                        FireScoreDidChange(_score);
                    break;
                }
                default:
                    stream.SkipProperty();
                    break;
            }
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */
