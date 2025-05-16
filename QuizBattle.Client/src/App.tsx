import { Outlet } from 'react-router-dom';
import Header from './components/Layout/Header';
import './styles/App.css';

const App = () => {
  return (
    <div className="app">
      <Header />
      <main className="app-content">
        <Outlet />
      </main>
    </div>
  );
};

export default App;