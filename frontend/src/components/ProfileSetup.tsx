import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/profile.css';
import { Form, Button, Container, Row, Col, Alert } from 'react-bootstrap';
import Loading from './Loading';

interface Profile {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  bio: string;
  address: string;
  phoneNumber: string;
}

const ProfileSetup: React.FC = () => {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axios.get('https://localhost:7073/api/Profile');
        setProfile(response.data);
      } catch (err) {
        if (axios.isAxiosError(err)) {
          setError(err.message);
        } else {
          setError('An unexpected error occurred');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setProfile({ ...profile, [e.target.name]: e.target.value } as Profile);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.put('https://localhost:7073/api/Profile', profile);
      setSuccess('Profile updated successfully!');
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(err.message);
      } else {
        setError('An unexpected error occurred');
      }
    }
  };

  if (loading) return <Loading />;
  if (error) return <Alert variant="danger">Error: {error}</Alert>;

  return (
    <Container className="profile-setup mt-4">
      <h2>Profile Setup</h2>
      {success && <Alert variant="success">{success}</Alert>}
      <Form onSubmit={handleSubmit}>
        <Row>
          <Col>
            <Form.Group controlId="firstName">
              <Form.Label>First Name</Form.Label>
              <Form.Control
                type="text"
                name="firstName"
                value={profile?.firstName || ''}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
          <Col>
            <Form.Group controlId="lastName">
              <Form.Label>Last Name</Form.Label>
              <Form.Control
                type="text"
                name="lastName"
                value={profile?.lastName || ''}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
        </Row>
        <Row>
          <Col>
            <Form.Group controlId="dateOfBirth">
              <Form.Label>Date of Birth</Form.Label>
              <Form.Control
                type="date"
                name="dateOfBirth"
                value={profile?.dateOfBirth || ''}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
          <Col>
            <Form.Group controlId="phoneNumber">
              <Form.Label>Phone Number</Form.Label>
              <Form.Control
                type="text"
                name="phoneNumber"
                value={profile?.phoneNumber || ''}
                onChange={handleChange}
              />
            </Form.Group>
          </Col>
        </Row>
        <Form.Group controlId="bio">
          <Form.Label>Bio</Form.Label>
          <Form.Control
            as="textarea"
            rows={3}
            name="bio"
            value={profile?.bio || ''}
            onChange={handleChange}
          />
        </Form.Group>
        <Form.Group controlId="address">
          <Form.Label>Address</Form.Label>
          <Form.Control
            type="text"
            name="address"
            value={profile?.address || ''}
            onChange={handleChange}
          />
        </Form.Group>
        <Button variant="primary" type="submit" className="mt-3">
          Save Profile
        </Button>
      </Form>
    </Container>
  );
};

export default ProfileSetup;
