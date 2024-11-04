import React from "react";
import { Routes, Route } from "react-router-dom";
import Home from "./trial/pages/Home";
import About from "./trial/pages/About";
import ContactForm from "./trial/components/ContactForm";

const App: React.FC = () => (
  <>
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      <Route path="/contact" element={<ContactForm />} />
    </Routes>
  </>
);

export default App;
