﻿using FrameInterfaces;
using Interfaces.Misc;

namespace Utils
{
    public abstract class InitializableBase : IInitializable
    {
        private readonly object _initLock = new object();
        protected readonly IMyLogger _logger;
        private InitializationStates _initializationState;

        protected InitializableBase(IMyLogger logger)
        {
            _logger = logger;
        }


        public bool Initiailze()
        {
            if (IsInitialized)
            {
                _logger.Info("Already initialized");
                return IsInitialized;
            }

            Monitor.Enter(_initLock);
            try
            {
                if (InitializationState == InitializationStates.Initialized)
                {
                    _logger.Info("Already initialized");
                    return IsInitialized;
                }
                InitializationState = InitializationStates.Initializing;
                
                InternalInit();
                
                InitializationState = InitializationStates.Initialized;
            }
            catch (Exception ex)
            {
                InitializationState = InitializationStates.InitializationFailed;
                _logger.Error($"Exception occurred: {ex}");
            }
            finally
            {
                Monitor.Exit(_initLock);
            }

            _logger.Info($"Internal initialization finished. New state: {InitializationState}");
            return IsInitialized;
        }


        public void Close()
        {
            if (!IsInitialized)
            {
                _logger.Info("Not initialized yet");
                return;
            }

            Monitor.Enter(_initLock);
            try
            {
                if (!IsInitialized)
                {
                    _logger.Info("Not initialized yet");
                    return;
                }

                InitializationState = InitializationStates.Closing;

                InternalClose();

                InitializationState = InitializationStates.NotInitialized;
            }
            catch (Exception ex)
            {
                InitializationState = InitializationStates.InitializationFailed;
                _logger.Error($"Exception occurred: {ex}");
            }
            finally
            {
                Monitor.Exit(_initLock);
            }
            _logger.Info($"Internal close finished. New state: {InitializationState}");
        }


        public bool IsInitialized => InitializationState == InitializationStates.Initialized;

        public InitializationStates InitializationState
        {
            get => _initializationState;
            protected set
            {

                InitializationStates oldState = _initializationState;
                _initializationState = value;

                InitStateChanged?.Invoke(this, new InitializationEventArgs(_initializationState, oldState));
            }
        }


        public event EventHandler<InitializationEventArgs> InitStateChanged;

        #region protected

        protected abstract void InternalInit();

        protected abstract void InternalClose();

        #endregion
    }
}
