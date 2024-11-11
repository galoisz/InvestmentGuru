import React from "react";
import { observer } from "mobx-react-lite";
import portfolioStore from "../stores/portfolioStore";

const Home: React.FC = observer(() => <div>{portfolioStore.title}</div>);

export default Home;
