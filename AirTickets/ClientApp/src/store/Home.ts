import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface HomeState {
    isLoading: boolean;
    startDateIndex?: number;
    xmlFiles: XmlFile[];
}

export interface XmlFile {
    id: number;
    fileName: string;
    filePath: string;
    summary: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestXmlFilesAction {
    type: 'REQUEST_XML_FILES';
    startDateIndex: number;
}

interface ReceiveXmlFilesAction {
    type: 'RECEIVE_XML_FILES';
    startDateIndex: number;
    xmlFiles: XmlFile[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestXmlFilesAction | ReceiveXmlFilesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestXmlFiles: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.home && startDateIndex !== appState.home.startDateIndex) {
            fetch(`home`)
                .then(response => response.json() as Promise<XmlFile[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_XML_FILES', startDateIndex: startDateIndex, xmlFiles: data });
                });

            dispatch({ type: 'REQUEST_XML_FILES', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: HomeState = { xmlFiles: [], isLoading: false };

export const reducer: Reducer<HomeState> = (state: HomeState | undefined, incomingAction: Action): HomeState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_XML_FILES':
            return {
                startDateIndex: action.startDateIndex,
                xmlFiles: state.xmlFiles,
                isLoading: true
            };
        case 'RECEIVE_XML_FILES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    xmlFiles: action.xmlFiles,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
