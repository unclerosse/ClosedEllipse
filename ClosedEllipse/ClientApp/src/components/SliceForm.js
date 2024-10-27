import React, { useState } from 'react';
import "./ObjectForm.css";
import EllipsoidComponent from './EllipsoidComponent';

export default function SliceForm() {
    const [x, setX] = useState(0);
    const [y, setY] = useState(0);
    const [z, setZ] = useState(0);
    const [semiAxisA, setSemiAxisA] = useState(0);
    const [semiAxisB, setSemiAxisB] = useState(0);
    const [eulerAngleX, setEulerAngleX] = useState(0);
    const [eulerAngleY, setEulerAngleY] = useState(0);
    const [eulerAngleZ, setEulerAngleZ] = useState(0);
    
    const [result, setResult] = useState(null);
    const [ellipsoids, setEllipsoids] = useState(null);

    const handleSubmit = (event) => {
        event.preventDefault();

        const ellipsoid = {
            x: parseFloat(x),
            y: parseFloat(y),
            z: parseFloat(z),
            semiAxisA: parseFloat(semiAxisA),
            semiAxisB: parseFloat(semiAxisB),
            eulerAngleX: parseFloat(eulerAngleX),
            eulerAngleY: parseFloat(eulerAngleY),
            eulerAngleZ: parseFloat(eulerAngleZ)
        };
    
        fetch('/get-slices', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(ellipsoid),
        })
            .then(async response => {
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

        setEllipsoids([ellipsoid]);
    }


    return (
        <form onSubmit={handleSubmit}>
            <div className='input-data'>
                <label>
                    X: 
                </label>
                <input className='input-field' type="number" value={x} onChange={(event) => setX(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    Y: 
                </label>
                <input className='input-field' type="number" value={y} onChange={(event) => setY(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    Z: 
                </label>
                <input className='input-field' type="number" value={z} onChange={(event) => setZ(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    semiAxisA: 
                </label>
                <input className='input-field' type="number" value={semiAxisA} onChange={(event) => setSemiAxisA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    semiAxisB: 
                </label>
                <input className='input-field' type="number" value={semiAxisB} onChange={(event) => setSemiAxisB(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleX: 
                </label>
                <input className='input-field' type="number" value={eulerAngleX} onChange={(event) => setEulerAngleX(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleY: 
                </label>
                <input className='input-field' type="number" value={eulerAngleY} onChange={(event) => setEulerAngleY(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleZ: 
                </label>
                <input className='input-field' type="number" value={eulerAngleZ} onChange={(event) => setEulerAngleZ(event.target.value)}/>
            </div>

            <button type="submit">
                <span className="loading-spinner">Submit</span>
            </button>

            <EllipsoidComponent ellipsoids={ellipsoids} points={result}/>
        </form>
    )
};

