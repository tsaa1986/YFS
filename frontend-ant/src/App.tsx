import React, { useEffect, useState } from 'react';
import { Navigate, Routes, Route, Link, useNavigate } from 'react-router-dom';
import { Login } from './components/AccountManagement/Login';
import { Main } from './components/MainLayout/Main';


const App: React.FC = () => {
  const navigate = useNavigate();
  const [isLoggedIn, setisLoggedIn] = useState(false);

  useEffect(() => {
    // Checking if user is not loggedIn
    if (!isLoggedIn) {
      navigate("/");
    } else {
      navigate("/login");
    }
  }, [navigate, isLoggedIn]);


  return (
    <Routes>
      <Route path='/' element={<Main />} />
      <Route path='/login' element={<Login />} />
      <Route path='/accounts' element={<div>accounts</div>} />
      {/*<Route path='*' element={<Navigate to='/' />} />*/}
    </Routes>
  );
};


export default App;
