import React, { Component } from 'react';
import ObjectForm from './ObjectForm';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
    <div>
      <h1>Please, enter your data</h1>
      <ObjectForm/>
    </div>
    );
  }
}
