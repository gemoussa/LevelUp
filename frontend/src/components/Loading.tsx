import React from 'react';
import { RingLoader } from 'react-spinners';
import '../styles/loading.css';

const Loading: React.FC = () => {
  return (
    <div className="loading-screen">
      <RingLoader color="#12B4A9" size={250} speedMultiplier={0.5} />
    </div>
  );
};

export default Loading;
