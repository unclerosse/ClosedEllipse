import React, { useEffect, createRef } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import { TrackballControls } from 'three/examples/jsm/controls/TrackballControls';

const EllipsoidComponent = ({ ellipsoids, points }) => {
  const containerRef = createRef();

  useEffect(() => {
    if (!ellipsoids) {
      return;
    }
    console.log(ellipsoids);

    const container = containerRef.current;
    const width = container.clientWidth;
    const height = container.clientHeight;

    const scene = new THREE.Scene();
    scene.background = new THREE.Color(0xffffff);

    const camera = new THREE.PerspectiveCamera(75, width / height, 0.001, 1000000000);
    camera.position.set(0, 0, 50);

    const renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(width, height);
    container.appendChild(renderer.domElement);

    const controls = new OrbitControls(camera, renderer.domElement);
    const trackballControls = new TrackballControls(camera, renderer.domElement);

    const axesHelper = new THREE.AxesHelper(16);
    scene.add(axesHelper);

    scene.add(new THREE.AmbientLight(0x666666));

    const light = new THREE.PointLight(0xffffff, 20);
		camera.add( light );

    ellipsoids.forEach((ellipsoidParams) => {
      const {
        x, y, z, semiAxisA, semiAxisB, eulerAngleX, eulerAngleY, eulerAngleZ,
      } = ellipsoidParams;
      const ecc = semiAxisB / semiAxisA;

      const geometry = new THREE.SphereGeometry(semiAxisA, 16, 16);
      const material = new THREE.MeshBasicMaterial({ color: 0x00ff00, wireframe: true });
      const ellipsoid = new THREE.Mesh(geometry, material);

      ellipsoid.position.set(x, y, z);
      ellipsoid.scale.set(1, ecc, ecc);
      ellipsoid.rotation.set(eulerAngleX, eulerAngleY, eulerAngleZ);

      scene.add(ellipsoid);
    });

    if (points) {
      points.forEach((pointParams) => {
        const {
          x, y, z
        } = pointParams;

        var pointGeometry = new THREE.BufferGeometry();
        pointGeometry.setAttribute( 'position', new THREE.Float32BufferAttribute([x, y, z], 3));
        var pointMaterial = new THREE.PointsMaterial({ size: 0.1, color: 0xff0000 });
        var point = new THREE.Points( pointGeometry, pointMaterial );
        scene.add(point);
      })
    }

    const animate = () => {
      requestAnimationFrame(animate);

      controls.update();
      trackballControls.update();    

      renderer.render(scene, camera);
    };

    animate();

    return () => {
      container.removeChild(renderer.domElement);
      renderer.dispose();
      controls.dispose();
      trackballControls.dispose();
    };
  }, [ellipsoids, points]);

  return (<div style={{height: '600px'}} ref={containerRef}/>);
};

export default EllipsoidComponent;