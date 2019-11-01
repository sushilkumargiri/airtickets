import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface OutputDatasState {
    isLoading: boolean;
    startDateIndex?: number;
    outputData: OutputData[];
}

export interface OutputData {
    fileName: string;
    id: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestOutputDatasAction {
    type: 'REQUEST_OUTPUT_DATA';
    startDateIndex: number;
}

interface ReceiveOutputDatasAction {
    type: 'RECEIVE_OUTPUT_DATA';
    startDateIndex: number;
    outputData: OutputData[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestOutputDatasAction | ReceiveOutputDatasAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestOutputDatas: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.outputDatas && startDateIndex !== appState.outputDatas.startDateIndex) {
            fetch(`outputData`)
                .then(response => response.json() as Promise<OutputData[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_OUTPUT_DATA', startDateIndex: startDateIndex, outputData: data });
                });

            dispatch({ type: 'REQUEST_OUTPUT_DATA', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: OutputDatasState = { outputData: [], isLoading: false };

export const reducer: Reducer<OutputDatasState> = (state: OutputDatasState | undefined, incomingAction: Action): OutputDatasState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_OUTPUT_DATA':
            return {
                startDateIndex: action.startDateIndex,
                outputData: state.outputData,
                isLoading: true
            };
        case 'RECEIVE_OUTPUT_DATA':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    outputData: action.outputData,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
