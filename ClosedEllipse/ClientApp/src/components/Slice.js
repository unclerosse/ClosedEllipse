import React, { Component } from 'react';
import SliceForm from './SliceForm';


export class Slice extends Component {
  static displayName = Slice.name;

  render() {
    return (
    <div>
      <h1>Please, enter your data</h1>
      <SliceForm/>
    </div>
    );
  }
}
