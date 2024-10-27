import React, { Component } from 'react';
import IntersectionForm from './IntersectionForm';


export class Intersection extends Component {
  static displayName = Intersection.name;

  render() {
    return (
    <div>
      <h1>Please, enter your data</h1>
      <IntersectionForm/>
    </div>
    );
  }
}
