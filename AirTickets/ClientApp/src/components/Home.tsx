import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as HomeStore from '../store/Home';

// At runtime, Redux will merge together...
type HomeProps =
    HomeStore.HomeState // ... state we've requested from the Redux store
    & typeof HomeStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class Home extends React.PureComponent<HomeProps> {
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
            <p><strong>appsettings.json </strong>file contains input directory path. Change it accordingly.</p>
            <p>
                <Link className="btn btn-info" to="/fetch-data">Convert XML to HTML</Link>
            </p>
        {this.renderXMLFilesTable()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
      this.props.requestXmlFiles(startDateIndex);
  }

  private renderXMLFilesTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Xml Input File</th>
          </tr>
        </thead>
            <tbody>
                {this.props.xmlFiles.map((xFile: HomeStore.XmlFile) =>
                    <tr key={xFile.id}>
                        <td>
                            {xFile.fileName}
                        </td>
            </tr>
          )}
        </tbody>
      </table>
    );
    }

}

export default connect(
    (state: ApplicationState) => state.home, // Selects which state properties are merged into the component's props
    HomeStore.actionCreators // Selects which action creators are merged into the component's props
)(Home as any);
