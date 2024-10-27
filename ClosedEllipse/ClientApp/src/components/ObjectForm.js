import React, { useState } from 'react';
import "./ObjectForm.css";
import EllipsoidComponent from './EllipsoidComponent';

export default function ObjectForm() {
  const [numberOfItems, setNumberOfItems] = useState(1);
  const [nc, setNC] = useState(0);
  const [eccentricity, setEccentricity] = useState(0);
  const [semiAxisDistribution, setDistribution] = useState('gauss');
  const [semiMinor, setSemiMinor] = useState(0);
  const [semiMajor, setSemiMajor] = useState(0);
  const [semiAxisShape, setSemiAxisShape] = useState(0);
  const [semiAxisScale, setSemiAxisScale] = useState(0);
  const [rglobal, setRglobal] = useState(0);
  const [volumeType, setVolumeType] = useState('sphere');
  const [centerDistribution, setCenterDistribution] = useState('gauss');
  const [centerShape, setCenterShape] = useState(0);
  const [centerScale, setCenterScale] = useState(0);
  const [numberOfFiles, setNumberOfFiles] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [result, setResult] = useState(null);

  const handleSubmit = (event) => {
    event.preventDefault();

    const RequestDTO = {
      NumberOfItems: numberOfItems,
      NC: nc,
      Eccentricity: eccentricity,
      SemiAxisDistribution: semiAxisDistribution,
      SemiAxes: [],
      Rglobal: rglobal,
      VolumeType: volumeType,
      CenterDistribution: centerDistribution,
      Centers: [],
      NumberOfFiles: numberOfFiles,
    };

    if (semiAxisDistribution === 'uniform') {
      RequestDTO.SemiAxes = [semiMinor, semiMajor];
    } else {
      RequestDTO.SemiAxes = [semiAxisShape, semiAxisScale];
    }

    if (centerDistribution !== 'uniform') {
      RequestDTO.Centers = [centerShape, centerScale];
    } else {
      RequestDTO.Centers = [-1, 1]
    }
    
    setIsLoading(true);

    fetch('/generate', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(RequestDTO),
    })
      .then(async response => {
        setIsLoading(false); 
        if (response.ok) {
          return response.json();
        } else {
          return response.json().then(error => {
            alert(error);
          });
        }
      })
      .then(json => {
        console.log(json);
        setResult(json);
      })
      .catch(error => {
        console.error(error);
      });

    
  };

  const isDisabled = numberOfItems === "" || nc === "" || rglobal === "" ||
    eccentricity === "" || semiMinor === "" || semiMajor === "" || semiAxisShape === "" || 
    semiAxisScale === "" || centerShape === "" || centerScale === "" || numberOfFiles === "" || isLoading;

  return (
    <form onSubmit={handleSubmit}>
      <div className='input-data'>
        <label>
          Number of Items: 
        </label>
        <input className='input-field' type="number" value={numberOfItems} onChange={(event) => setNumberOfItems(event.target.value)}/>
      </div>
      
      <div className='input-data'>
        <label>
          NC: 
        </label>
        <input className='input-field' type="number" value={nc} onChange={(event) => {setNC(event.target.value); setRglobal(0);}}/>
      </div>
      

      <div className='input-data'>
        <label>
          Ecc: 
        </label>
        <input className='input-field' type="number" value={eccentricity} onChange={(event) => setEccentricity(event.target.value)}/>
      </div>
      
      <div className='input-data'>
        <label>
          Distribution: 
        </label>
        <select className='input-field' value={semiAxisDistribution} onChange={(event) => setDistribution(event.target.value)}>
          <option value="gauss">Gauss</option>
          <option value="gamma">Gamma</option>
          <option value="uniform">Uniform</option>
        </select>
        {semiAxisDistribution === 'uniform' && (
          <>
            
            <div className='input-data'>
              <label>
                Min: 
              </label>
              <input className='input-field' type="number" value={semiMinor} onChange={(event) => setSemiMinor(event.target.value)}/>
            </div>
            
            
            <div className='input-data'>
              <label>
                Max: 
              </label>
              <input className='input-field' type="number" value={semiMajor} onChange={(event) => setSemiMajor(event.target.value)}/>
            </div>
          </>
        )}
        {semiAxisDistribution !== 'uniform' && (
          <>
            
            <div className='input-data'>
              <label>
                Shape: 
              </label>
              <input className='input-field' type="number" value={semiAxisShape} onChange={(event) => setSemiAxisShape(event.target.value)}/>
            </div>
            
            
            <div className='input-data'>
              <label>
                Scale: 
              </label>
              <input className='input-field' type="number" value={semiAxisScale} onChange={(event) => setSemiAxisScale(event.target.value)}/>
            </div>
          </>
        )}
      </div>
    
      <div className='input-data'>
        <label>
          Rglobal: 
        </label>
        <input className='input-field' type="number" value={rglobal} onChange={(event) => {setRglobal(event.target.value); setNC(0);}}/>
      </div>
    
      <div className='input-data'>
        <label>
          Volume Type: 
        </label>
        <select className='input-field' value={volumeType} onChange={(event) => setVolumeType(event.target.value)}>
          <option value="cube">Cube</option>
          <option value="sphere">Sphere</option>
        </select>
      </div>

      <div className='input-data'>
        <label>
          Distribution for Centers: 
        </label>
        <select className='input-field' value={centerDistribution} onChange={(event) => setCenterDistribution(event.target.value)}>
          <option value="gauss">Gauss</option>
          <option value="gamma">Gamma</option>
          <option value="uniform">Uniform</option>
        </select>
        {centerDistribution !== 'uniform' && (
          <>
            <div className='input-data'>
              <label>
                Shape: 
              </label>
              <input className='input-field' type="number" value={centerShape} onChange={(event) => setCenterShape(event.target.value)}/>
            </div>  
            <div className='input-data'>
              <label>
                Scale: 
              </label>
              <input className='input-field' type="number" value={centerScale} onChange={(event) => setCenterScale(event.target.value)}/>
            </div>
          </>
        )}
      </div>
      
      <div className='input-data'>
        <label>
          Number of Files: 
        </label>
        <input className='input-field' type="number" value={numberOfFiles} onChange={(event) => setNumberOfFiles(event.target.value)}/>
      </div>
      
      <button type="submit" disabled={isDisabled}>
        {(isLoading && <span className="loading-spinner">Loading...</span>) || <span className="loading-spinner">Submit</span>}
      </button>
      <EllipsoidComponent ellipsoids={result}/>
      </form>
  );
}