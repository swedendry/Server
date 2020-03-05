import React, { useState } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col } from 'reactstrap';
import classnames from 'classnames';

export default function Main({ tabs }) {
    const [activeTab, setActiveTab] = useState(0);

    const toggle = tab => {
        if (activeTab !== tab) setActiveTab(tab);
    }

    return (
        <div>
            <Nav tabs>
                {tabs.map((tab, index) => (
                    <NavItem key={index}>
                        <NavLink
                            className={classnames({ active: activeTab === index })}
                            onClick={() => { toggle(index); }}
                        >
                            {tab.name}
                        </NavLink>
                    </NavItem>
                ))}
            </Nav>
            <TabContent activeTab={activeTab}>
                {tabs.map((tab, index) => (
                    <TabPane key={index} tabId={index}>
                        <Row>
                            <Col sm="12">
                                {tab.view}
                            </Col>
                        </Row>
                    </TabPane>
                ))}
            </TabContent>
        </div>
    );
}