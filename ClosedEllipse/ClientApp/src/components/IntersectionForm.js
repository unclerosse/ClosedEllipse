import React, { useState } from 'react';
import "./ObjectForm.css";
import EllipsoidComponent from './EllipsoidComponent';

export default function IntersectionForm() {
    const [ellipsoidA, setEllipsoidA] = useState(null);
    const [ellipsoidB, setEllipsoidB] = useState(null);
    
    const [result, setResult] = useState(null);
    const [points, setPoints] = useState(null);
    const [ellipsoids, setEllipsoids] = useState(null);

    const handleSubmit = (event) => {
        event.preventDefault();

        const request = {
            first: ellipsoidA,
            second: ellipsoidB
        };
    
        fetch('/check-intersection', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
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

        alert(result.Result);
        setPoints(result.FirstPoints.concat(result.SecondPoints));

        setEllipsoids([ellipsoidA, ellipsoidB]);
    }


    return (
        <form onSubmit={handleSubmit}>
            <div className='input-data'>
                <label>
                    X: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.x} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    Y: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.y} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    Z: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.z} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    semiAxisA: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.semiAxisA} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    semiAxisB: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.semiAxisB} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleX: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.eulerAngleX} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleY: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.eulerAngleY} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>
            <div className='input-data'>
                <label>
                    eulerAngleZ: 
                </label>
                <input className='input-field' type="number" value={ellipsoidA.eulerAngleZ} onChange={(event) => setEllipsoidA(event.target.value)}/>
            </div>

            <button type="submit">
                <span className="loading-spinner">Submit</span>
            </button>

            <EllipsoidComponent ellipsoids={ellipsoids} points={points}/>
        </form>
    )
};

