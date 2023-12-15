import React, { Component } from 'react';
import * as THREE from 'three';

export class DrawEllipsoid extends Component {
  constructor(props) {
    super(props);
    this.canvasRef = React.createRef();
  }

  componentDidMount() {
    const canvas = this.canvasRef.current;
    const renderer = new THREE.WebGLRenderer({ canvas });

    const fov = 60;
    const aspect = canvas.clientWidth / canvas.clientHeight;
    const near = 0.1;
    const far = 1000;
    const camera = new THREE.PerspectiveCamera(fov, aspect, near, far);
    camera.position.z = 5;

    const scene = new THREE.Scene();
    scene.background = new THREE.Color(0xffffff);

    const geometry = new THREE.SphereGeometry(1);
    const material = new THREE.MeshBasicMaterial({ color: 0xff32ff, wireframe: true });
    const ellipsoid = new THREE.Mesh(geometry, material);
    ellipsoid.scale.set(2, 1, 1);
    // ellipsoid.position.x = 10;
    // ellipsoid.position.y = 15;
    // ellipsoid.position.z = -4;


    const point = new THREE.Vector3(0, 0, 0);
    const pointGeometry = new THREE.BufferGeometry().setFromPoints([point]);
    const pointMaterial = new THREE.PointsMaterial({ color: 0xff32ff, size: 1, sizeAttenuation: false });
    const pointMesh = new THREE.Points(pointGeometry, pointMaterial);

    scene.add(ellipsoid);
    // scene.add(pointMesh)

    const animate = () => {
      requestAnimationFrame(animate);
      // ellipsoid.rotation.z += 0.01;
      ellipsoid.rotation.y += 0.01;
      // pointMesh.position.x -= 0.1;
      // if (pointMesh.position.x <= -10) {
      //   pointMesh.position.x = 0;
      // }
      renderer.render(scene, camera);
    };
    animate();
  }

  render() {
    return <canvas ref={this.canvasRef} />;
  }
}