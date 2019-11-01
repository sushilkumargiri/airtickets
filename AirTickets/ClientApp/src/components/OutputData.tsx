import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as OutputDatasStore from '../store/OutputData';

// At runtime, Redux will merge together...
type outputDataProps =
    OutputDatasStore.OutputDatasState // ... state we've requested from the Redux store
    & typeof OutputDatasStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class FetchData extends React.PureComponent<outputDataProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
            <p>
                <Link className="btn btn-info" to="/">go back</Link>
            </p>
        {this.renderoutputDatasTable()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
    this.props.requestOutputDatas(startDateIndex);
  }

  private renderoutputDatasTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Output HTML file</th>
          </tr>
        </thead>
            <tbody>
                {this.props.outputData.map((outputData: OutputDatasStore.OutputData) =>
            <tr key={outputData.id}>
              <td>{outputData.fileName}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

}

export default connect(
    (state: ApplicationState) => state.outputDatas, // Selects which state properties are merged into the component's props
  OutputDatasStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
