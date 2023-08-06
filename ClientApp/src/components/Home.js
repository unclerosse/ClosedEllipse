import React, { Component } from 'react';
import styled from "styled-components";


async function getData(Rgl, a, b) {
  if (!isFinite(Rgl) || !isFinite(a) || !isFinite(b)) {
    console.log('here');
    Rgl = -1;
    a = -1;
    b = -1;
  }
  const response = await fetch(`http://localhost:5050/generate?Rgl=${Rgl}&a=${a}&b=${b}`);
  const data = await response.json();

  return data;
}

const Button = styled.button`
  background-color: black;
  color: white;
  padding: 5px 15px;
  border-radius: 5px;
  outline: 0;
  margin: 10px 0px;
  cursor: pointer;
  box-shadow: 0px 2px 2px lightgray;
  transition: ease background-color 250ms;
  &:hover {
    background-color: gray;
  }
`;

const TextField = styled.input`
  background-color: white;
  color: black;
  padding: 5px 15px;
  border-radius: 5px;
  outline: 0;
  margin: 10px 0px;
  cursor: pointer;
  box-shadow: 0px 2px 2px lightgray;
  transition: ease background-color 250ms;
  &:hover {
    background-color: light-gray;
  }
`;


export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <form>
          Rgl: <TextField type='text' name='Rgl' id='Rgl'></TextField>
          <br></br>
          a: <TextField type='text' name='a' id='a'></TextField>
          <br></br>
          b: <TextField type='text' name='b' id='b'></TextField>
        </form>
        
        <Button onClick={async () => {
            const Rgl = document.getElementById('Rgl').value;
            const a = document.getElementById('a').value;
            const b = document.getElementById('b').value;

            const response = await getData(Rgl, a, b); 

            document.getElementById("response").innerHTML = response;
          }}>Generate Object</Button>
        
        <br></br>
        <h id="response">Try add any data!</h>
      </div>
    );
  }
}
