import { Home } from "./components/Home";
import { Intersection } from "./components/Intersection";
import { Slice } from "./components/Slice";

const AppRoutes = [
  {
    index: true,
    path: '/',
    element: <Home />
  },
  {
    index: true,
    path: '/slice',
    element: <Slice />
  },
  {
    index: true,
    path: '/intersection',
    element: <Intersection/>
  }
];

export default AppRoutes;
